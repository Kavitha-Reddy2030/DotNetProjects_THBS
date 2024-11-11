using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserRoleAPI.DataAccessLayer.Models
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "RoleName is required.")]
        [MaxLength(255, ErrorMessage = "RoleName cannot exceed 255 characters.")]
        public string RoleName { get; set; }

        [Required(ErrorMessage = "CreatedOn is required.")]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "CreatedBy is required.")]
        public string CreatedBy { get; set; }

        [Required(ErrorMessage = "ActiveStatus is required.")]
        public bool ActiveStatus { get; set; } = true;

        public ICollection<User> Users { get; set; }
    }
}
