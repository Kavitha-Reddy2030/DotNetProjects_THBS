using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PackageAPI.DTOs
{
    public class CreatePackageDTO
    {
        [Required]
        public string PackageName { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        public decimal Price { get; set; }
        
        [Required]
        public List<CreatePackageCityDTO> PackageCities { get; set; }

        [Required]
        public List<CreatePackageResortDTO> PackageResorts { get; set; }
    }

    

    
}
