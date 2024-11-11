using UserRoleAPI.DataAccessLayer.Models;

namespace UserRoleAPI.Logging.Interfaces
{
    public interface IExportExcelService
    {
        Task<byte[]> ExportUsersToExcelAsync(IEnumerable<User> users);
        Task<byte[]> ExportRolesToExcelAsync(IEnumerable<Role> roles);
    }
}