using UserRoleAPI.DataAccessLayer.ModelValidations;

namespace UserRoleAPI.DataAccessLayer.DTO
{
    public class UpdateUserDTO
    {
        [UpperCaseUserName]
        public string? UserName { get; set; }
        public string? Email { get; set; }
        [StrongPassword]
        public string? Password { get; set; }
        public string? MobileNumber { get; set; }
        public int? RoleId { get; set; }
        public bool? ActiveStatus { get; set; } = true;
    }
}
