using PackageAPI.DTO;
using PackageAPI.DTOs;
using PackageAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageAPI.BusinessLayer
{
  public interface IPackageService
  {
    Task<IEnumerable<PackageDTO>> GetAllPackagesAsync();
    Task<PackageDTO> GetPackageByIdAsync(int packageId);
    Task<Package> CreatePackageAsync(PostPackageDTO packageDto);
    Task<Package> UpdatePackageAsync(int packageId, PostPackageDTO packageDto);
    Task<bool> DeletePackageAsync(int packageId);
  }
}
