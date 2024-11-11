using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using UserRoleAPI.DataAccessLayer.DTO;
using UserRoleAPI.DataAccessLayer.Models;
using UserRoleAPI.DataAccessLayer.Repositories;

namespace UserRoleAPI.BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogService _logService;

        public UserService(IUserRepository userRepository, IMapper mapper, IEmailService emailService, ILogService logService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _emailService = emailService;
            _logService = logService;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            _logService.Log("Information", "Fetching all users.");
            var users = await _userRepository.GetAllUsersAsync();
            var userDtos = _mapper.Map<IEnumerable<UserDTO>>(users);
            _logService.Log("Information", $"Successfully fetched {userDtos.Count()} users.");
            return userDtos;
        }

        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            _logService.Log("Information", $"Fetching user with ID: {userId}");
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logService.Log("Warning", $"User with ID {userId} not found.");
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }
            var userDto = _mapper.Map<UserDTO>(user);
            _logService.Log("Information", $"Successfully fetched user with ID: {userId}");
            return userDto;
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            _logService.Log("Information", $"Fetching user with email: {email}");
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                _logService.Log("Warning", $"User with email {email} not found.");
            }
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<CreateUserDTO> CreateUserAsync(CreateUserDTO userDto, string currentUserRole)
        {
            _logService.Log("Information", $"Creating user with email: {userDto.Email} by {currentUserRole}");
            string actualPassword = userDto.Password;
            userDto.Password = HashPassword(actualPassword);

            var user = _mapper.Map<User>(userDto);
            var createdUser = await _userRepository.CreateUserAsync(user);

            // Send email notification, including the current user's role
            await _emailService.SendEmailNotificationAsync(user, actualPassword, currentUserRole);
            _logService.Log("Information", $"Successfully created user with ID: {createdUser.UserId}");

            return _mapper.Map<CreateUserDTO>(createdUser);
        }

        public async Task UpdateUserAsync(int userId, UpdateUserDTO userUpdateDto)
        {
            _logService.Log("Information", $"Updating user with ID: {userId}");
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                _logService.Log("Warning", $"User with ID {userId} not found for update.");
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            // Update only the fields that are provided
            if (!string.IsNullOrWhiteSpace(userUpdateDto.UserName))
            {
                existingUser.UserName = userUpdateDto.UserName;
            }
            if (!string.IsNullOrWhiteSpace(userUpdateDto.Email))
            {
                existingUser.Email = userUpdateDto.Email;
            }
            if (userUpdateDto.RoleId.HasValue)
            {
                existingUser.RoleId = userUpdateDto.RoleId.Value;
            }
            if (!string.IsNullOrWhiteSpace(userUpdateDto.MobileNumber))
            {
                existingUser.MobileNumber = userUpdateDto.MobileNumber;
            }
            if (userUpdateDto.ActiveStatus.HasValue)
            {
                existingUser.ActiveStatus = userUpdateDto.ActiveStatus.Value;
            }
            if (!string.IsNullOrWhiteSpace(userUpdateDto.Password))
            {
                existingUser.Password = HashPassword(userUpdateDto.Password);
            }

            await _userRepository.UpdateUserAsync(existingUser);
            _logService.Log("Information", $"Successfully updated user with ID: {userId}");
        }

        public async Task DeleteUserAsync(int userId)
        {
            _logService.Log("Information", $"Deleting user with ID: {userId}");
            await _userRepository.DeleteUserAsync(userId);
            _logService.Log("Information", $"Successfully deleted user with ID: {userId}");
        }

        public async Task<bool> VerifyPasswordAsync(string email, string password)
        {
            _logService.Log("Information", $"Verifying password for user with email: {email}");
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                _logService.Log("Warning", $"User with email {email} not found for password verification.");
                return false;
            }

            bool isValid = VerifyHashedPassword(user.Password, password);
            _logService.Log("Information", $"Password verification for user with email {email} was {(isValid ? "successful" : "unsuccessful")}");
            return isValid;
        }

        private bool VerifyHashedPassword(string hashedPassword, string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedInputPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
                return hashedPassword == hashedInputPassword;
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
                return hashedPassword;
            }
        }
    }
}
