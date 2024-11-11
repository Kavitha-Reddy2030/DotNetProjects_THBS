using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PackageAPI_UOW.Models.Domain
{
    public class PackageCity
    {
        [Key]
        public int PackageCityId { get; set; } // Primary Key for the bridge table
         [Required]
        public string Description { get; set; } // Description for the Package-City relationship

        [ForeignKey("Package")]
        public int PackageId { get; set; } // Foreign key
        public Package Package { get; set; } // Navigation property

        [ForeignKey("City")]
        public int CityId { get; set; } // Foreign key
        public City City { get; set; } // Navigation property
    }
}
