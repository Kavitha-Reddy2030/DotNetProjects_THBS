using FluentEmail.Core;
using UserRoleAPI.DataAccessLayer.Models;

namespace UserRoleAPI.BusinessLayer.Services
{
    public class EmailService : IEmailService
    {
        private readonly IFluentEmail _fluentEmail;

        public EmailService(IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail;
        }

        public async Task SendEmailNotificationAsync(User user, string actualPassword, string currentUserRole)
        {
            string subject = "Posting your Login Credentials to login to our application";
            string body = $"Hello {user.UserName},\n\n" +
                           $"Your account has been created successfully by a {currentUserRole}.\n" +
                          $"Email: {user.Email}\n" +
                          $"Password: {actualPassword}\n" +
                          "Please log in at your earliest convenience.\n\n" +
                          "Thanks and Regards,\n" +
                           $"{currentUserRole} Team";

            await _fluentEmail
                .To(user.Email, user.UserName)
                .Subject(subject)
                .Body(body)
                .SendAsync();
        }
    }
}

