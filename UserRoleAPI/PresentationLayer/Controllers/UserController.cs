using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserRoleAPI.BusinessLayer.Services;
using UserRoleAPI.DataAccessLayer.DTO;
using UserRoleAPI.DataAccessLayer.Repositories;
using UserRoleAPI.Logging.Interfaces;

namespace UserRoleAPI.PresentationLayer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IExportExcelService _excelExportService;
        private readonly IExportPdfService _pdfExportService;
        private readonly ILogService _logService;
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository, IUserService userService, ILogService logService, IExportExcelService excelExportService, IExportPdfService pdfExportService)
        {
            _userRepository = userRepository;
            _userService = userService;
            _logService = logService;
            _excelExportService = excelExportService;
            _pdfExportService = pdfExportService;
        }

        [HttpGet]
        [Authorize(Roles = "Super Admin, Admin")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            _logService.Log("Information", "Fetching all users.");
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logService.Log("Error", "An error occurred while fetching the users in the database.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while fetching the users." });
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Super Admin, Admin")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            _logService.Log("Information", $"Attempting to retrieve user by ID: {id}");
            try
            {
                var user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                {
                    _logService.Log("Warning", $"User with ID {id} not found.");
                    return NotFound(new { message = $"User with ID {id} not found." });
                }

                _logService.Log("Information", $"Successfully retrieved user with ID: {id}");
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                _logService.Log("Warning", $"Key not found: {ex.Message}");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logService.Log("Error", "An error occurred in the database while fetching the user.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while fetching the user." });
            }
        }

        [HttpGet("{email}")]
        [Authorize(Roles = "Super Admin, Admin")]
        public async Task<ActionResult<UserDTO>> GetUserByEmail(string email)
        {
            _logService.Log("Information", $"Attempting to retrieve user by email: {email}");
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);

                if (user == null)
                {
                    _logService.Log("Warning", $"User with email '{email}' not found.");
                    return NotFound(new { message = $"User with email '{email}' not found." });
                }

                _logService.Log("Information", $"Successfully retrieved user with email: {email}");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logService.Log("Error", "An error occurred while trying to fetch the user from the database.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while trying to fetch the user." });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin, Admin")]
        public async Task<ActionResult<CreateUserDTO>> CreateUser(CreateUserDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                _logService.Log("Warning", "Model state is invalid for user creation.");
                return BadRequest(ModelState);
            }

            try
            {
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var createdUserDto = await _userService.CreateUserAsync(userDto, currentUserRole);

                _logService.Log("Information", $"User created successfully with ID: {createdUserDto.UserId}");
                return CreatedAtAction(nameof(GetUserById), new { id = createdUserDto.UserId },
                    new { message = "User created successfully.", user = createdUserDto });
            }
            catch (ArgumentException ex)
            {
                _logService.Log("Warning", $"Argument exception: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logService.Log("Error", "An error occurred while creating the user.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while creating the user." });
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDTO userUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                _logService.Log("Warning", "Model state is invalid for user update.");
                return BadRequest(ModelState);
            }

            try
            {
                await _userService.UpdateUserAsync(id, userUpdateDto);
                _logService.Log("Information", $"User with ID: {id} updated successfully.");
                return Ok(new { message = "User updated successfully." });
            }
            catch (InvalidOperationException ex)
            {
                _logService.Log("Warning", $"Invalid operation: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logService.Log("Error", "An error occurred while updating the user in the database.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while updating the user." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            _logService.Log("Information", $"Attempting to delete user with ID: {id}");
            try
            {
                await _userService.DeleteUserAsync(id);
                _logService.Log("Information", $"User with ID: {id} deleted successfully.");
                return Ok(new { message = "User deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                _logService.Log("Warning", $"User not found for deletion: {ex.Message}");
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logService.Log("Error", "An error occurred while trying to delete the user from the database.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "An error occurred while trying to delete the user." });
            }
        }

        [HttpGet("export-excel")]
        public async Task<IActionResult> ExportUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            var excelData = await _excelExportService.ExportUsersToExcelAsync(users);
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Users.xlsx");
        }

        [HttpGet("export-pdf")]
        public async Task<IActionResult> ExportUsersToPdf()
        {
            var users = await _userRepository.GetAllUsersAsync();
            var pdfData = await _pdfExportService.ExportUsersToPdfAsync(users);
            return File(pdfData, "application/pdf", "Users.pdf");
        }
    }
}
