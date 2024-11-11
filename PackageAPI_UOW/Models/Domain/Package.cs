using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PackageAPI_UOW.Models.Domain
{
    public class Package
    {
        [Key]
        public int PackageId { get; set; } // Primary Key
        [Required]
        public string PackageName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }

       //[JsonIgnore]
        public virtual ICollection<PackageCity> PackageCities { get; set; }
        public virtual ICollection<PackageResort> PackageResorts { get; set; }
    }
}
