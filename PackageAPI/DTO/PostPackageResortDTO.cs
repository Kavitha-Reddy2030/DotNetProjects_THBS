using System.ComponentModel.DataAnnotations;

namespace PackageAPI{
    public class PostPackageResortDTO
{
    [Required]
    public string ResortName { get; set; }

    [Required]
    public string CityName { get; set; } 
}

}