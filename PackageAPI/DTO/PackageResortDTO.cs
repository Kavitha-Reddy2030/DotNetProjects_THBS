namespace PackageAPI.DTO{
    public class PackageResortDTO
{
    //public int Id { get; set; }
    //public int PackageId { get; set; }
    public int ResortId { get; set; }
    public string StateName { get; set; }
    public string CountryName { get; set; }
    public string Description { get; set; }
    public ResortDTO Resort { get; set; }  // This holds resort details like name, city, state, country
}

}