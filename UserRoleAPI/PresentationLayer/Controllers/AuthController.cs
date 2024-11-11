using Microsoft.AspNetCore.Mvc;
using UserRoleAPI.BusinessLayer.Services;
using UserRoleAPI.DataAccessLayer.DTO;

namespace UserRoleAPI.BusinessLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly TokenService _tokenService;

        public AuthController(IUserService userService, TokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.GetUserByEmailAsync(loginDto.Email);
            if (user == null)
            {
                // Log or debug to check if the user was not found
                return Unauthorized(new { message = "Invalid email or password - user not found" });
            }

            // Check if passwords match
            var isPasswordValid = await _userService.VerifyPasswordAsync(loginDto.Email, loginDto.Password);
            if (!isPasswordValid)
            {
                // Log or debug to verify password issue
                return Unauthorized(new { message = "Invalid email or password - password mismatch" });
            }
            if (user.RoleName != "Super Admin" && user.RoleName != "Admin")
            {
                return Unauthorized(new { message = "Access restricted to Super Admin and Admin roles." });
            }
            // Generate JWT token for the user
            var token = _tokenService.GenerateToken(user.Email, user.RoleName);
            return Ok(new { token });
        }
    }
}