using Microsoft.EntityFrameworkCore;
using UserRoleAPI.Data;
using UserRoleAPI.DataAccessLayer.Models;
using UserRoleAPI.BusinessLayer.Services;

namespace UserRoleAPI.DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserRoleDbContext _context;
        private readonly ILogService _logService;

        public UserRepository(UserRoleDbContext context, ILogService logService)
        {
            _context = context;
            _logService = logService;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            _logService.Log("Information", "Fetching all active users from the database.");
            var users = await _context.Users
                .Where(u => u.ActiveStatus)
                .OrderByDescending(u => u.CreatedOn)
                .Include(u => u.Role)
                .ToListAsync();
            _logService.Log("Information", $"Successfully fetched {users.Count} active users.");
            return users;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            _logService.Log("Information", $"Fetching user with ID: {userId}");
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                _logService.Log("Warning", $"User with ID {userId} not found.");
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }
            _logService.Log("Information", $"Successfully fetched user with ID: {userId}");
            return user;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            if (user == null)
            {
                _logService.Log("Error", "Attempted to create a null user.");
                throw new ArgumentException("User cannot be null.", nameof(user));
            }

            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                _logService.Log("Error", $"Email '{user.Email}' already exists. Cannot create user.");
                throw new ArgumentException($"Email '{user.Email}' already exists. Please choose a different email.", nameof(user));
            }

            _logService.Log("Information", $"Creating user with email: {user.Email}");
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            _logService.Log("Information", $"Successfully created user with ID: {user.UserId}");
            return user;
        }

        public async Task UpdateUserAsync(User user)
        {
            if (user == null)
            {
                _logService.Log("Error", "Attempted to update a null user.");
                throw new InvalidOperationException("User cannot be null.");
            }

            if (!await _context.Users.AnyAsync(u => u.UserId == user.UserId))
            {
                _logService.Log("Error", $"User with ID {user.UserId} not found for update.");
                throw new InvalidOperationException($"User with ID {user.UserId} not found.");
            }

            if (await _context.Users.AnyAsync(u => u.Email == user.Email && u.ActiveStatus && u.UserId != user.UserId))
            {
                _logService.Log("Error", $"Email '{user.Email}' already exists for an active user.");
                throw new InvalidOperationException($"Email '{user.Email}' already exists for an active user. Please choose a different email.");
            }

            _logService.Log("Information", $"Updating user with ID: {user.UserId}");
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            _logService.Log("Information", $"Successfully updated user with ID: {user.UserId}");
        }

        public async Task DeleteUserAsync(int userId)
        {
            _logService.Log("Information", $"Deleting user with ID: {userId}");
            var user = await GetUserByIdAsync(userId);
            if (user == null)
            {
                _logService.Log("Warning", $"User with ID {userId} not found during delete operation.");
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            user.ActiveStatus = false;
            await UpdateUserAsync(user);
            _logService.Log("Information", $"Successfully marked user with ID: {userId} as inactive.");
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            _logService.Log("Information", $"Fetching user with email: {email}");
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email && u.ActiveStatus);

            if (user != null)
            {
                _logService.Log("Information", $"Successfully fetched user with email: {email}");
            }
            else
            {
                _logService.Log("Warning", $"No active user found with email: {email}");
            }
            return user;
        }
    }
}


