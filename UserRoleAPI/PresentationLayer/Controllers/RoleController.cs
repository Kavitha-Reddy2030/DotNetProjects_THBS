using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserRoleAPI.BusinessLayer.Services;
using UserRoleAPI.DataAccessLayer.DTO;
using UserRoleAPI.DataAccessLayer.Repositories;
using UserRoleAPI.Logging.Interfaces;

namespace UserRoleAPI.BusinessLayer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogService _logService;
        private readonly IRoleRepository _roleRepository;
        private readonly IExportExcelService _excelExportService;
        private readonly IExportPdfService _pdfExportService;

        public RoleController(
            IRoleRepository roleRepository,
            IRoleService roleService,
            ILogService logService,
            IExportExcelService excelExportService,
            IExportPdfService pdfExportService)
        {
            _roleRepository = roleRepository;
            _roleService = roleService;
            _logService = logService;
            _excelExportService = excelExportService;
            _pdfExportService = pdfExportService;
        }

        [HttpGet]
        [Authorize(Roles = "Super Admin, Admin")]
        public async Task<ActionResult<IEnumerable<RoleDTO>>> GetAllRoles()
        {
            _logService.Log("Information", "Fetching all roles.");
            try
            {
                var roles = await _roleService.GetAllRolesAsync();
                _logService.Log("Information", "Successfully retrieved all roles.");
                return Ok(roles);
            }
            catch (KeyNotFoundException ex)
            {
                _logService.Log("Warning", "No roles found.", ex);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logService.Log("Error", "An error occurred in database while retrieving roles.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving roles." });
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Super Admin, Admin")]
        public async Task<ActionResult<RoleDTO>> GetRoleById(int id)
        {
            _logService.Log("Information", $"Fetching role with ID: {id}");
            try
            {
                var role = await _roleService.GetRoleByIdAsync(id);
                if (role == null)
                {
                    _logService.Log("Warning", $"Role with ID {id} not found.");
                    return NotFound();
                }

                _logService.Log("Information", $"Successfully retrieved role with ID: {id}");
                return Ok(role);
            }
            catch (KeyNotFoundException ex)
            {
                _logService.Log("Warning", $"Role with ID {id} not found.", ex);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logService.Log("Error", $"An error occurred while processing your request for role ID {id}in the database.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while processing your request." });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin")]
        public async Task<ActionResult<RoleDTO>> CreateRole(RoleDTO roleDto)
        {
            if (!ModelState.IsValid)
            {
                _logService.Log("Warning", "Invalid model state for creating role.", null, ModelState);
                return BadRequest(ModelState);
            }

            _logService.Log("Information", $"Creating role with data: {roleDto}");
            try
            {
                var createdRoleDto = await _roleService.CreateRoleAsync(roleDto);
                _logService.Log("Information", $"Successfully created role with ID: {createdRoleDto.RoleId}");
                return CreatedAtAction(nameof(GetRoleById), new { id = createdRoleDto.RoleId }, new { message = "Role created successfully.", role = createdRoleDto });
            }
            catch (InvalidOperationException ex)
            {
                _logService.Log("Error", "Error creating role.", ex);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logService.Log("Error", "An error occurred while processing your request to create a role in the database.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while processing your request." });
            }
        }

        [HttpPut("{roleId}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> UpdateRole(int roleId, [FromBody] UpdateRoleDTO updateRoleDto)
        {
            if (!ModelState.IsValid)
            {
                _logService.Log("Warning", $"Invalid model state for updating role with ID {roleId}.", null, ModelState);
                return BadRequest(ModelState);
            }

            _logService.Log("Information", $"Updating role with ID: {roleId}");
            try
            {
                await _roleService.UpdateRoleAsync(roleId, updateRoleDto);
                _logService.Log("Information", $"Successfully updated role with ID: {roleId}");
                return Ok(new { message = "Role updated successfully." });
            }
            catch (InvalidOperationException ex)
            {
                _logService.Log("Error", $"Error updating role with ID {roleId}.", ex);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logService.Log("Error", $"An error occurred while updating the role with ID {roleId} in the database.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while updating the role." });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            _logService.Log("Information", $"Attempting to delete role with ID: {id}");
            try
            {
                await _roleService.DeleteRoleAsync(id);
                _logService.Log("Information", $"Successfully deleted role with ID: {id}");
                return Ok(new { message = "Role deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                _logService.Log("Warning", $"Role with ID {id} not found during delete operation.", ex);
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logService.Log("Error", $"An error occurred while deleting the role with ID {id} in the database.", ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while deleting the role." });
            }
        }

        [HttpGet("export-excel")]
        public async Task<IActionResult> ExportRoles()
        {
            _logService.Log("Information", "Exporting roles to Excel.");
            var roles = await _roleRepository.GetAllRolesAsync();
            var excelData = await _excelExportService.ExportRolesToExcelAsync(roles);
            _logService.Log("Information", "Roles successfully exported to Excel.");
            return File(excelData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Roles.xlsx");
        }

        [HttpGet("export-pdf")]
        public async Task<IActionResult> ExportRolesToPdf()
        {
            _logService.Log("Information", "Exporting roles to PDF.");
            var roles = await _roleRepository.GetAllRolesAsync();
            var pdfData = await _pdfExportService.ExportRolesToPdfAsync(roles);
            _logService.Log("Information", "Roles successfully exported to PDF.");
            return File(pdfData, "application/pdf", "Roles.pdf");
        }
    }
}


