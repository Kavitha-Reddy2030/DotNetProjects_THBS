namespace PackageAPI_UOW.Models.DTO{
    public class PackageDTO
{
    public int PackageId { get; set; }
    public string PackageName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public List<PackageCityDTO> PackageCities { get; set; }
    public List<PackageResortDTO> PackageResorts { get; set; }
}
}