using System.ComponentModel.DataAnnotations;

namespace PackageAPI_UOW.Models.Domain
{
    public class City
    {
        [Key]
        public int CityId { get; set; } // Primary Key
        [Required]
        public string CityName { get; set; }

        
        [Required]
        public int StateId { get; set; } // Foreign Key
        public State State { get; set; } // Navigation property


         public ICollection<Resort> Resorts { get; set; }
         public ICollection<PackageCity> PackageCities { get; set; }
    }
}
