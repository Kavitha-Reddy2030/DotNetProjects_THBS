using UserRoleAPI.DataAccessLayer.Models;

namespace UserRoleAPI.BusinessLayer.Services
{
    public interface IEmailService
    {
        Task SendEmailNotificationAsync(User user, string actualPassword, string currentUserRole);
    }
}