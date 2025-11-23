using AwningsAPI.Database;
using AwningsAPI.Dto.Auth;
using AwningsAPI.Interfaces;
using AwningsAPI.Model.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
//using Org.BouncyCastle.Crypto.Generators;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AwningsAPI.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto, string ipAddress)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username && u.IsActive);

            string hash = HashPassword(loginDto.Password);

            if (user == null || !VerifyPasswordHash(loginDto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            // Update last login
            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Generate tokens
            var jwtToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            // Save refresh token
            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.UserId,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedByIp = ipAddress
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role ?? "User",
                Department = user.Department,
                Token = jwtToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // Check if username already exists
            if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
            {
                throw new InvalidOperationException("Username already exists");
            }

            // Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            var user = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                Username = registerDto.Username,
                PasswordHash = HashPassword(registerDto.Password),
                Role = "User",
                Department = registerDto.Department,
                IsActive = true,
                DateCreated = DateTime.UtcNow,
                CreatedBy = "System"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Generate tokens
            var jwtToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            // Save refresh token
            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.UserId,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role ?? "User",
                Department = user.Department,
                Token = jwtToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, string ipAddress)
        {
            var token = await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && !rt.IsRevoked);

            if (token == null || token.ExpiresAt < DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Invalid or expired refresh token");
            }

            // Revoke old token
            token.IsRevoked = true;

            // Generate new tokens
            var jwtToken = GenerateJwtToken(token.User);
            var newRefreshToken = GenerateRefreshToken();

            // Save new refresh token
            var newRefreshTokenEntity = new RefreshToken
            {
                UserId = token.UserId,
                Token = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedByIp = ipAddress
            };

            _context.RefreshTokens.Add(newRefreshTokenEntity);
            await _context.SaveChangesAsync();

            return new AuthResponseDto
            {
                UserId = token.User.UserId,
                Username = token.User.Username,
                Email = token.User.Email,
                FirstName = token.User.FirstName,
                LastName = token.User.LastName,
                Role = token.User.Role ?? "User",
                Department = token.User.Department,
                Token = jwtToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };
        }

        public async Task<bool> RevokeTokenAsync(string refreshToken)
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (token == null) return false;

            token.IsRevoked = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            if (!VerifyPasswordHash(changePasswordDto.CurrentPassword, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Current password is incorrect");
            }

            user.PasswordHash = HashPassword(changePasswordDto.NewPassword);
            user.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        //public async Task<User?> GetUserByIdAsync(int userId)
        //{
        //    return await _context.Users.FindAsync(userId);
        //}

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        //public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        //{
        //    return await _context.Users
        //        .Select(u => new UserDto
        //        {
        //            UserId = u.UserId,
        //            FirstName = u.FirstName,
        //            LastName = u.LastName,
        //            Email = u.Email,
        //            Username = u.Username,
        //            Role = u.Role ?? "User",
        //            Department = u.Department,
        //            IsActive = u.IsActive,
        //            LastLogin = u.LastLogin
        //        })
        //        .ToListAsync();
        //}

        public string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPasswordHash(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }


        // Add these methods to your existing AuthService.cs

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToListAsync();

            return users.Select(u => new UserDto
            {
                UserId = u.UserId,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Username = u.Username,
                Role = u.Role,
                Department = u.Department,
                IsActive = u.IsActive,
                DateCreated = u.DateCreated,
                CreatedBy = u.CreatedBy
            });
        }

        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return null;
            }

            return new UserDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
                Role = user.Role,
                Department = user.Department,
                IsActive = user.IsActive,
                DateCreated = user.DateCreated,
                CreatedBy = user.CreatedBy
            };
        }

        public async Task<UserDto> UpdateUserAsync(int userId, UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new Exception($"User with ID {userId} not found");
            }

            // Update only provided fields
            if (!string.IsNullOrEmpty(dto.FirstName))
                user.FirstName = dto.FirstName;

            if (!string.IsNullOrEmpty(dto.LastName))
                user.LastName = dto.LastName;

            if (!string.IsNullOrEmpty(dto.Email))
            {
                // Check if email is already taken by another user
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == dto.Email && u.UserId != userId);

                if (existingUser != null)
                {
                    throw new Exception("Email is already in use");
                }
                user.Email = dto.Email;
            }

            if (!string.IsNullOrEmpty(dto.Department))
                user.Department = dto.Department;

            if (!string.IsNullOrEmpty(dto.Role))
                user.Role = dto.Role;

            if (dto.IsActive.HasValue)
                user.IsActive = dto.IsActive.Value;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
                Role = user.Role,
                Department = user.Department,
                IsActive = user.IsActive,
                DateCreated = user.DateCreated,
                CreatedBy = user.CreatedBy
            };
        }

        public async Task<bool> DeactivateUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return false;
            }

            user.IsActive = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ActivateUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return false;
            }

            user.IsActive = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }


    }
}