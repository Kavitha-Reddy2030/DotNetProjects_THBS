using System.ComponentModel.DataAnnotations;

namespace PackageAPI{
    public class UpdatePackageCityDTO
{
    [Required]
    public string CityName { get; set; }

    [Required]
    public string StateName { get; set; }

    [Required]
    public string CountryName { get; set; }
    
     [Required]
     public string Description { get; set; }
}

}