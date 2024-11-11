namespace PackageAPI_UOW.Models.DTO{
    public class PackageCityDTO
{
    public int CityId { get; set; }
    public string Description { get; set; }
    public CityDTO City { get; set; }
}
}