using System.ComponentModel.DataAnnotations;

namespace PackageAPI{
    public class UpdatePackageDTO
{
    [Required]
    public string PackageName { get; set; }

    public string Description { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public List<UpdatePackageCityDTO> PackageCities { get; set; }

    [Required]
    public List<UpdatePackageResortDTO> PackageResorts { get; set; }
}

}