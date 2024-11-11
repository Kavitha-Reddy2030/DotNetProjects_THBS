using UserRoleAPI.DataAccessLayer.DTO;

namespace UserRoleAPI.BusinessLayer.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<CreateUserDTO> CreateUserAsync(CreateUserDTO userDto, string currentUserRole);
        Task UpdateUserAsync(int userId, UpdateUserDTO userUpdateDto);
        Task DeleteUserAsync(int userId);
        Task<UserDTO> GetUserByEmailAsync(string Email);
        Task<bool> VerifyPasswordAsync(string email, string password);
    }
}