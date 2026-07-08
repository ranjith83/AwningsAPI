using AwningsAPI.Dto.Auth;
using AwningsAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AwningsAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IAuthService authService, ILogger<UsersController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _authService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _authService.GetUserByIdAsync(id);
            if (user == null) return NotFound(new { message = $"User with ID {id} not found" });
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UpdateUserDto dto)
        {
            var user = await _authService.UpdateUserAsync(id, dto);
            return Ok(user);
        }

        [HttpPatch("{id}/deactivate")]
        public async Task<ActionResult> DeactivateUser(int id)
        {
            var result = await _authService.DeactivateUserAsync(id);
            if (!result) return NotFound(new { message = $"User with ID {id} not found" });
            return Ok(new { message = "User deactivated successfully" });
        }

        [HttpPatch("{id}/activate")]
        public async Task<ActionResult> ActivateUser(int id)
        {
            var result = await _authService.ActivateUserAsync(id);
            if (!result) return NotFound(new { message = $"User with ID {id} not found" });
            return Ok(new { message = "User activated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var result = await _authService.DeleteUserAsync(id);
            if (!result) return NotFound(new { message = $"User with ID {id} not found" });
            return Ok(new { message = "User deleted successfully" });
        }
    }
}
