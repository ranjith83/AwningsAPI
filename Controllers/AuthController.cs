using AwningsAPI.Dto.Auth;
using AwningsAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AwningsAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var response = await _authService.LoginAsync(loginDto, ipAddress);
            _logger.LogInformation("User {Username} logged in from {IP}", loginDto.Username, ipAddress);
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
        {
            var response = await _authService.RegisterAsync(registerDto);
            _logger.LogInformation("New user registered: {Username}", registerDto.Username);
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var response = await _authService.RefreshTokenAsync(refreshTokenDto.RefreshToken, ipAddress);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenDto refreshTokenDto)
        {
            await _authService.RevokeTokenAsync(refreshTokenDto.RefreshToken);
            _logger.LogInformation("User {User} logged out", User?.Identity?.Name);
            return Ok(new { message = "Logged out successfully" });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _authService.ChangePasswordAsync(userId, changePasswordDto);
            if (result)
                return Ok(new { message = "Password changed successfully" });
            return BadRequest(new { message = "Failed to change password" });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user = await _authService.GetUserByIdAsync(userId);
            if (user == null) return NotFound(new { message = "User not found" });

            return Ok(new UserDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Username = user.Username,
                Role = user.Role ?? "User",
                Department = user.Department,
                IsActive = user.IsActive
            });
        }

        [Authorize]
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _authService.GetAllUsersAsync();
            return Ok(users);
        }



        /// <summary>
        /// Get user by ID (Admin only)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _authService.GetUserByIdAsync(id);
            if (user == null) return NotFound(new { message = $"User with ID {id} not found" });
            return Ok(user);
        }

        /// <summary>
        /// Update user information (Admin only)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UpdateUserDto dto)
        {
            var user = await _authService.UpdateUserAsync(id, dto);
            return Ok(user);
        }

        /// <summary>
        /// Deactivate user (Admin only)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/deactivate")]
        public async Task<ActionResult> DeactivateUser(int id)
        {
            var result = await _authService.DeactivateUserAsync(id);
            if (!result) return NotFound(new { message = $"User with ID {id} not found" });
            return Ok(new { message = "User deactivated successfully" });
        }

        /// <summary>
        /// Activate user (Admin only)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/activate")]
        public async Task<ActionResult> ActivateUser(int id)
        {
            var result = await _authService.ActivateUserAsync(id);
            if (!result) return NotFound(new { message = $"User with ID {id} not found" });
            return Ok(new { message = "User activated successfully" });
        }

        /// <summary>
        /// Delete user permanently (Admin only)
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var result = await _authService.DeleteUserAsync(id);
            if (!result) return NotFound(new { message = $"User with ID {id} not found" });
            return Ok(new { message = "User deleted successfully" });
        }

        /// <summary>
        /// Get all salespeople (users with department "Sales")
        /// </summary>
        [Authorize]
        [HttpGet("salespeople")]
        public async Task<ActionResult<IEnumerable<SalespersonDto>>> GetSalespeople()
        {
            var salespeople = await _authService.GetSalespeopleAsync();
            return Ok(salespeople);
        }
    }
}