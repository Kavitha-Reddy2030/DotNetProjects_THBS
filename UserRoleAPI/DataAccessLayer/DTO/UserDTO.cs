using UserRoleAPI.DataAccessLayer.ModelValidations;

namespace UserRoleAPI.DataAccessLayer.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        [UpperCaseUserName]
        public string UserName { get; set; }
        public string Email { get; set; }
        [StrongPassword]
        public string Password { get; set; }
        public string? MobileNumber { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; }
        public bool ActiveStatus { get; set; } = true;
    }
}