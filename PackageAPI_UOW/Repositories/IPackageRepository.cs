using PackageAPI_UOW.Models.Domain;

namespace PackageAPI_UOW.Repositories{
    public interface IPackageRepository
{
    Task<IEnumerable<Package>> GetAllPackagesAsync();
    Task<Package> GetPackageByIdAsync(int packageId);
    Task AddPackageAsync(Package package);
    Task UpdatePackageAsync(Package package);
    Task DeletePackageAsync(int packageId);
}

}