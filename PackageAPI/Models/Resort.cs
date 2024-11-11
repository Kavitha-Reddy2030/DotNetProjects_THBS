using System.ComponentModel.DataAnnotations;

namespace PackageAPI.Models
{
    public class Resort
    {
        [Key]
        public int ResortId { get; set; } // Primary Key

        [Required]
        public string ResortName { get; set; }

       

        [Required]
        public int CityId { get; set; } // Foreign Key
        public City City { get; set; } // Navigation property

        public ICollection<PackageResort> PackageResorts { get; set; }
    }
}
