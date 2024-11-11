
using UserRoleAPI.DataAccessLayer.Models;

namespace UserRoleAPI.Logging.Interfaces
{
    public interface IExportPdfService
    {
        Task<byte[]> ExportUsersToPdfAsync(IEnumerable<User> users);
        Task<byte[]> ExportRolesToPdfAsync(IEnumerable<Role> roles);
    }
}