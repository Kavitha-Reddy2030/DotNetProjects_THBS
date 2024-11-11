namespace PackageAPI_UOW.Models.DTO{
    public class PackageResortDTO
{
    public int ResortId { get; set; }
    public string Description { get; set; }
    public ResortDTO Resort { get; set; }
}
}