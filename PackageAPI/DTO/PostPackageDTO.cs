using System.ComponentModel.DataAnnotations;

namespace PackageAPI{
    public class PostPackageDTO
{
    [Required]
    public string PackageName { get; set; }

    public string Description { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public List<PostPackageCityDTO> PackageCities { get; set; }

    [Required]
    public List<PostPackageResortDTO> PackageResorts { get; set; }
}

}