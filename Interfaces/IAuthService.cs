using AwningsAPI.Dto.Auth;
using AwningsAPI.Model.Auth;
using Microsoft.AspNetCore.Identity.Data;

namespace AwningsAPI.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto, string ipAddress);
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> RefreshTokenAsync(string refreshToken, string ipAddress);
        Task<bool> RevokeTokenAsync(string refreshToken);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
      //  Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserByUsernameAsync(string username);
       // Task<IEnumerable<UserDto>> GetAllUsersAsync();
        string GenerateJwtToken(User user);
        string GenerateRefreshToken();

       // Task LogoutAsync(string refreshToken);

        // User Management
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<UserDto> UpdateUserAsync(int userId, UpdateUserDto dto);
        Task<bool> DeactivateUserAsync(int userId);
        Task<bool> ActivateUserAsync(int userId);
        Task<bool> DeleteUserAsync(int userId);

    }
}