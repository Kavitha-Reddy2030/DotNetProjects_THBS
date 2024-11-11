using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using UserRoleAPI.DataAccessLayer.ModelValidations;

namespace UserRoleAPI.DataAccessLayer.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required(ErrorMessage = "UserName is required.")]
        [MaxLength(255, ErrorMessage = "UserName cannot exceed 255 characters.")]
        [UpperCaseUserName]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(255, ErrorMessage = "Password cannot exceed 255 characters.")]
        [StrongPassword]
        public string Password { get; set; }

        [MaxLength(20, ErrorMessage = "MobileNumber cannot exceed 20 characters.")]
        [RegularExpression(@"^0?[789]\d{9}$", ErrorMessage = "Mobile number must start with 7, 8, or 9 and be 10 digits long.")]
        public string? MobileNumber { get; set; } 

        [Required(ErrorMessage = "RoleId is required.")]
        public int RoleId { get; set; }

        [JsonIgnore]
        public Role Role { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "CreatedBy is required.")]
        public string CreatedBy { get; set; }

        [Required]
        public bool ActiveStatus { get; set; } = true;
    }
}
