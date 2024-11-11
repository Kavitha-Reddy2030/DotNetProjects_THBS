// using Microsoft.EntityFrameworkCore;
// using UserRoleAPI.BusinessLayer.Services;
// using UserRoleAPI.Data;
// using UserRoleAPI.DataAccessLayer.Models;

// namespace UserRoleAPI.DataAccessLayer.Repositories
// {
//     public class RoleRepository : IRoleRepository
//     {
//         private readonly UserRoleDbContext _context;
//         private readonly ILogService _logService;

//         public RoleRepository(UserRoleDbContext context, ILogService logService)
//         {
//             _context = context;
//             _logService = logService;
//         }

//         public async Task<IEnumerable<Role>> GetAllRolesAsync()
//         {
//             _logService.Log("Information", "Fetching all roles from the database.");
//             var roles = await _context.Roles
//                 .Where(r => r.ActiveStatus)
//                 .OrderByDescending(u => u.CreatedOn)
//                 .ToListAsync();
//             _logService.Log("Information", "Successfully fetched {RoleCount} active roles.", roles.Count);
//             return roles;
//         }

//         public async Task<Role> GetRoleByIdAsync(int roleId)
//         {
//             _logService.Log("Information", "Fetching role with ID: {RoleId}", roleId);
//             var role = await _context.Roles.FindAsync(roleId);
//             if (role == null)
//             {
//                 _logService.Log("Warning", "Role with ID {RoleId} not found.", roleId);
//                 throw new KeyNotFoundException($"Role with ID {roleId} not found.");
//             }
//             _logService.Log("Information", "Successfully fetched role with ID: {RoleId}", roleId);
//             return role;
//         }

//         public async Task<Role> CreateRoleAsync(Role role)
//         {
//             if (role == null)
//             {
//                 _logService.Log("Error", "Attempted to create a null role.");
//                 throw new InvalidOperationException("Role cannot be null.");
//             }

//             // Check if a role with the same name and active status already exists
//             if (await _context.Roles.AnyAsync(r => r.RoleName == role.RoleName && r.ActiveStatus))
//             {
//                 _logService.Log("Error", "Role name '{RoleName}' already exists as an active role.", role.RoleName);
//                 throw new InvalidOperationException($"Role name '{role.RoleName}' already exists as an active role.");
//             }
//             _logService.Log("Information", "Creating role with name: {RoleName}", role.RoleName);
//             await _context.Roles.AddAsync(role);
//             await _context.SaveChangesAsync();
//             _logService.Log("Information", "Successfully created role with ID: {RoleId}", role.RoleId);
//             return role;
//         }

//         public async Task UpdateRoleAsync(Role role)
//         {
//             if (role == null)
//             {
//                 _logService.Log("Error", "Attempted to update a null role.");
//                 throw new InvalidOperationException("Role cannot be null.");
//             }

//             // Check if the role with the specified ID exists
//             var existingRole = await _context.Roles.FindAsync(role.RoleId);
//             if (existingRole == null)
//             {
//                 _logService.Log("Error", "Role with ID {RoleId} not found for update.", role.RoleId);
//                 throw new InvalidOperationException($"Role with ID {role.RoleId} not found.");
//             }

//             // Check if an active role with the same name already exists
//             if (await _context.Roles.AnyAsync(r => r.RoleName == role.RoleName && r.ActiveStatus && r.RoleId != role.RoleId))
//             {
//                 _logService.Log("Error", "Role name '{RoleName}' already exists as an active role.", role.RoleName);
//                 throw new InvalidOperationException($"Role name '{role.RoleName}' already exists as role.");
//             }
//             _logService.Log("Information", "Updating role with ID: {RoleId}", role.RoleId);
//             _context.Roles.Update(role);
//             await _context.SaveChangesAsync();
//             _logService.Log("Information", "Successfully updated role with ID: {RoleId}", role.RoleId);
//         }

//         public async Task DeleteRoleAsync(int roleId)
//         {
//             _logService.Log("Information", "Deleting role with ID: {RoleId}", roleId);
//             var role = await GetRoleByIdAsync(roleId);
//             if (role == null)
//             {
//                 _logService.Log("Warning", "Role with ID {RoleId} not found during delete operation.", roleId);
//                 throw new KeyNotFoundException($"Role with ID {roleId} not found.");
//             }
//             role.ActiveStatus = false; 
//             await UpdateRoleAsync(role);
//             _logService.Log("Information", "Successfully marked role with ID: {RoleId} as inactive.", roleId);
//         }
//     }
// }


using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using UserRoleAPI.BusinessLayer.Services;
using UserRoleAPI.Data;
using UserRoleAPI.DataAccessLayer.Models;

namespace UserRoleAPI.DataAccessLayer.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly UserRoleDbContext _context;
        private readonly ILogService _logService;

        public RoleRepository(UserRoleDbContext context, ILogService logService)
        {
            _context = context;
            _logService = logService;
        }

        private void HandleSqlException(SqlException ex)
        {
            _logService.Log("Error", "A SQL error occurred.", ex);
            switch (ex.Number)
            {
                case 2601: // Unique constraint violation
                    throw new InvalidOperationException("A role with the same name already exists.");
                case 1205: // Deadlock victim
                    throw new InvalidOperationException("A deadlock occurred while accessing the database. Please try again.");
                case 547: // Foreign key constraint violation
                    throw new InvalidOperationException("The role cannot be deleted because it is being referenced by another entity.");
                case 8152: // String or binary data would be truncated (Data type mismatch)
                    throw new InvalidOperationException("A data type mismatch occurred. One of the fields exceeds the allowed size or is incompatible.");
                case 245: // Conversion failed due to data type mismatch
                    throw new InvalidOperationException("A data type mismatch occurred while converting data. Please check the input data types.");
                default:
                    throw new InvalidOperationException("An error occurred while accessing the database.");
            }
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            _logService.Log("Information", "Fetching all roles from the database.");
            try
            {
                var roles = await _context.Roles
                    .Where(r => r.ActiveStatus)
                    .OrderByDescending(u => u.CreatedOn)
                    .ToListAsync();
                _logService.Log("Information", "Successfully fetched {RoleCount} active roles.", roles.Count);
                return roles;
            }
            catch (SqlException ex)
            {
                HandleSqlException(ex);
                return null; // This will never be reached due to the exception being thrown
            }
        }

        public async Task<Role> GetRoleByIdAsync(int roleId)
        {
            _logService.Log("Information", "Fetching role with ID: {RoleId}", roleId);
            try
            {
                var role = await _context.Roles.FindAsync(roleId);
                if (role == null)
                {
                    _logService.Log("Warning", "Role with ID {RoleId} not found.", roleId);
                    throw new KeyNotFoundException($"Role with ID {roleId} not found.");
                }
                _logService.Log("Information", "Successfully fetched role with ID: {RoleId}", roleId);
                return role;
            }
            catch (SqlException ex)
            {
                HandleSqlException(ex);
                return null; // This will never be reached due to the exception being thrown
            }
        }

        public async Task<Role> CreateRoleAsync(Role role)
        {
            if (role == null)
            {
                _logService.Log("Error", "Attempted to create a null role.");
                throw new InvalidOperationException("Role cannot be null.");
            }

            try
            {
                // Check if a role with the same name and active status already exists
                if (await _context.Roles.AnyAsync(r => r.RoleName == role.RoleName && r.ActiveStatus))
                {
                    _logService.Log("Error", "Role name '{RoleName}' already exists as an active role.", role.RoleName);
                    throw new InvalidOperationException($"Role name '{role.RoleName}' already exists as an active role.");
                }

                _logService.Log("Information", "Creating role with name: {RoleName}", role.RoleName);
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
                _logService.Log("Information", "Successfully created role with ID: {RoleId}", role.RoleId);
                return role;
            }
            catch (SqlException ex)
            {
                HandleSqlException(ex);
                return null; // This will never be reached due to the exception being thrown
            }
        }

        public async Task UpdateRoleAsync(Role role)
        {
            if (role == null)
            {
                _logService.Log("Error", "Attempted to update a null role.");
                throw new InvalidOperationException("Role cannot be null.");
            }

            try
            {
                // Check if the role with the specified ID exists
                var existingRole = await _context.Roles.FindAsync(role.RoleId);
                if (existingRole == null)
                {
                    _logService.Log("Error", "Role with ID {RoleId} not found for update.", role.RoleId);
                    throw new InvalidOperationException($"Role with ID {role.RoleId} not found.");
                }

                // Check if an active role with the same name already exists
                if (await _context.Roles.AnyAsync(r => r.RoleName == role.RoleName && r.ActiveStatus && r.RoleId != role.RoleId))
                {
                    _logService.Log("Error", "Role name '{RoleName}' already exists as an active role.", role.RoleName);
                    throw new InvalidOperationException($"Role name '{role.RoleName}' already exists as role.");
                }

                _logService.Log("Information", "Updating role with ID: {RoleId}", role.RoleId);
                _context.Roles.Update(role);
                await _context.SaveChangesAsync();
                _logService.Log("Information", "Successfully updated role with ID: {RoleId}", role.RoleId);
            }
            catch (SqlException ex)
            {
                HandleSqlException(ex);
            }
        }

        public async Task DeleteRoleAsync(int roleId)
        {
            _logService.Log("Information", "Deleting role with ID: {RoleId}", roleId);
            try
            {
                var role = await GetRoleByIdAsync(roleId);
                if (role == null)
                {
                    _logService.Log("Warning", "Role with ID {RoleId} not found during delete operation.", roleId);
                    throw new KeyNotFoundException($"Role with ID {roleId} not found.");
                }
                role.ActiveStatus = false; // Soft delete
                await UpdateRoleAsync(role);
                _logService.Log("Information", "Successfully marked role with ID: {RoleId} as inactive.", roleId);
            }
            catch (SqlException ex)
            {
                HandleSqlException(ex);
            }
        }
    }
}
