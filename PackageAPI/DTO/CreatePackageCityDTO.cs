using System.ComponentModel.DataAnnotations;

namespace PackageAPI{
    public class CreatePackageCityDTO
    {
        [Required]
        public int CityId { get; set; }
        
        [Required]
        public string Description { get; set; }

         [Required]
        public string CityName { get; set; }
        
        [Required]
        public int StateId { get; set; } // Foreign Key

        [Required]
        public int CountryId { get; set; } // Foreign Key

        [Required]
        public string StateName { get; set; } // Optional, for additional info

        [Required]
        public string CountryName { get; set; } // Optional, for additional info

        // You can also include additional fields as needed
    }
}