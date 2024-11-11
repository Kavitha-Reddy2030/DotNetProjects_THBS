using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackageAPI.Models
{
    public class PackageCity
    {
        [Key]
        public int Id { get; set; } // Primary Key for the bridge table

        [ForeignKey("Package")]
        public int PackageId { get; set; } // Foreign key
        public Package Package { get; set; } // Navigation property

        [ForeignKey("City")]
        public int CityId { get; set; } // Foreign key
        public City City { get; set; } // Navigation property

        [Required]
        public string Description { get; set; } // Description for the Package-City relationship
    }
}
