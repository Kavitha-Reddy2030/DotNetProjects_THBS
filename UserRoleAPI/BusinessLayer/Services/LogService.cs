using Microsoft.EntityFrameworkCore;
using UserRoleAPI.Data;
using UserRoleAPI.DataAccessLayer.Models;

namespace UserRoleAPI.BusinessLayer.Services
{
    public class LogService : ILogService
    {
        private readonly UserRoleDbContext _context;

        public LogService(UserRoleDbContext context)
        {
            _context = context;
        }

        public void Log(string level, string message, object exception = null, object properties = null)
        {
            var logEntry = new Log
            {
                Timestamp = DateTime.UtcNow,
                Level = level,
                Message = message,
                MessageTemplate = message ?? "Default Message Template",
                Exception = exception != null ? exception.ToString() : "No exception provided.",
                Properties = properties != null ? properties.ToString() : "No properties provided."
            };

            _context.Logs.Add(logEntry);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException dbEx)
            {
                var innerExceptionMessage = dbEx.InnerException?.Message ?? "No inner exception.";
                var errorMessage = $"Logging failed: {dbEx.Message}. Inner Exception: {innerExceptionMessage}";

                Console.WriteLine(errorMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while logging: {ex.Message}");
            }
        }
    }
}
