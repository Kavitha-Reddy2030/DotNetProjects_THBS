using AutoMapper;
using UserRoleAPI.DataAccessLayer.DTO;
using UserRoleAPI.DataAccessLayer.Models;
using UserRoleAPI.DataAccessLayer.Repositories;

namespace UserRoleAPI.BusinessLayer.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        public RoleService(IRoleRepository roleRepository, IMapper mapper, ILogService logService)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logService = logService;
        }

        public async Task<IEnumerable<RoleDTO>> GetAllRolesAsync()
        {
            _logService.Log("Information", "Fetching all roles.");
            var roles = await _roleRepository.GetAllRolesAsync();
            var roleDtos = _mapper.Map<IEnumerable<RoleDTO>>(roles);
            _logService.Log("Information", $"Successfully fetched {roleDtos.Count()} roles.");
            return roleDtos;
        }

        public async Task<RoleDTO> GetRoleByIdAsync(int roleId)
        {
            _logService.Log("Information", $"Fetching role with ID: {roleId}");
            var role = await _roleRepository.GetRoleByIdAsync(roleId);
            if (role == null)
            {
                _logService.Log("Warning", $"Role with ID {roleId} not found.");
                throw new KeyNotFoundException($"Role with ID {roleId} not found.");
            }
            var roleDto = _mapper.Map<RoleDTO>(role);
            _logService.Log("Information", $"Successfully fetched role with ID: {roleId}");
            return roleDto;
        }

        public async Task<RoleDTO> CreateRoleAsync(RoleDTO roleDto)
        {
            _logService.Log("Information", $"Creating role with name: {roleDto.RoleName}");
            var role = _mapper.Map<Role>(roleDto);
            var createdRole = await _roleRepository.CreateRoleAsync(role);
            _logService.Log("Information", $"Successfully created role with ID: {createdRole.RoleId}");

            return _mapper.Map<RoleDTO>(createdRole);
        }

        public async Task UpdateRoleAsync(int roleId, UpdateRoleDTO updateRoleDto)
        {
            _logService.Log("Information", $"Updating role with ID: {roleId}");
            var existingRole = await _roleRepository.GetRoleByIdAsync(roleId);
            if (existingRole == null)
            {
                _logService.Log("Warning", $"Role with ID {roleId} not found for update.");
                throw new KeyNotFoundException($"Role with ID {roleId} not found.");
            }

            // Update only the fields that are provided
            if (!string.IsNullOrWhiteSpace(updateRoleDto.RoleName))
            {
                existingRole.RoleName = updateRoleDto.RoleName;
            }
            if (!string.IsNullOrWhiteSpace(updateRoleDto.CreatedBy))
            {
                existingRole.CreatedBy = updateRoleDto.CreatedBy;
            }
            if (updateRoleDto.ActiveStatus.HasValue)
            {
                existingRole.ActiveStatus = updateRoleDto.ActiveStatus.Value;
            }

            await _roleRepository.UpdateRoleAsync(existingRole);
            _logService.Log("Information", $"Successfully updated role with ID: {roleId}");
        }

        public async Task DeleteRoleAsync(int roleId)
        {
            _logService.Log("Information", $"Deleting role with ID: {roleId}");
            await _roleRepository.DeleteRoleAsync(roleId);
            _logService.Log("Information", $"Successfully deleted role with ID: {roleId}");
        }
    }
}

