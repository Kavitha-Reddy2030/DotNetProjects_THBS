

namespace PackageAPI.DTO{
  public class PackageDTO
{
    public int PackageId { get; set; }
    public string PackageName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
     public DateTime CreatedOn { get; set; } 
    public bool Status { get; set; } 

    public List<PackageCityDTO> PackageCities { get; set; }
    public List<PackageResortDTO> PackageResorts { get; set; }

        // public static implicit operator PackageDTO(PackageDTO v)
        // {
        //     throw new NotImplementedException();
        // }
    }
}
