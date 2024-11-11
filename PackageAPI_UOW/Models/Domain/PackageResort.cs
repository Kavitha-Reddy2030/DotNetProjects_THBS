using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackageAPI_UOW.Models.Domain
{
    public class PackageResort
    {
        [Key]
        public int PackageResortId { get; set; } // Primary Key for the bridge table
        [Required]
        public string Description { get; set; } // Description for the Package-Resort relationship

        [ForeignKey("Package")]
        public int PackageId { get; set; } // Foreign key
        public Package Package { get; set; } // Navigation property

        [ForeignKey("Resort")]
        public int ResortId { get; set; } // Foreign key
        public Resort Resort { get; set; } // Navigation property
    }
}
