namespace PackageAPI.DTO{
    public class PackageCityDTO
{
   // public int Id { get; set; }
    //public int PackageId { get; set; }
    public int CityId { get; set; }
    public string StateName { get; set; }
    public string CountryName { get; set; }
    public string Description { get; set; }
    public CityDTO City { get; set; }  // This holds city details like name, state, country
}
}