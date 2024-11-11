using UserRoleAPI.DataAccessLayer.DTO;

namespace UserRoleAPI.BusinessLayer.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDTO>> GetAllRolesAsync();
        Task<RoleDTO> GetRoleByIdAsync(int roleId);
        Task<RoleDTO> CreateRoleAsync(RoleDTO roleDto);
        Task UpdateRoleAsync(int roleId, UpdateRoleDTO updateRoleDto);
        Task DeleteRoleAsync(int roleId);
    }
}