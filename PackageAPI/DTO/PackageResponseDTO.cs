namespace PackageAPI.DTO{
    public class PackageResponseDTO
{
    // public string Message { get; set; }
    public int PackageId { get; set; }
    public string PackageName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public List<CityResponseDTO> PackageCities { get; set; }
    public List<ResortResponseDTO> PackageResorts { get; set; }
}
}