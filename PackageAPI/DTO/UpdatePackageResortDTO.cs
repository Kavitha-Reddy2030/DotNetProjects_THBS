
using System.ComponentModel.DataAnnotations;

namespace PackageAPI{
    public class UpdatePackageResortDTO
{
    [Required]
    public string ResortName { get; set; }

    [Required]
    public string CityName { get; set; } 

    [Required]
    public string Description { get; set; }
}

}

