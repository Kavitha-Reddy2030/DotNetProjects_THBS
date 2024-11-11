using System.ComponentModel.DataAnnotations;

namespace PackageAPI{
    public class PostPackageCityDTO
{
    public int CityId { get; set; }
    
    [Required]
    public string CityName { get; set; }

    [Required]
    public string StateName { get; set; }

    [Required]
    public string CountryName { get; set; }

   // [Required]
    // public string Description { get; set; }
}

}