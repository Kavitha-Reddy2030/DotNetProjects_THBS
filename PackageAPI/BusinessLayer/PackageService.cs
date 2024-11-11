using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PackageAPI.Data;
using PackageAPI.DTO;
using PackageAPI.DTOs;
using PackageAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PackageAPI.BusinessLayer
{
    public class PackageService : IPackageService
    {
        private readonly PackageDbContext _context;

        public PackageService(PackageDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<PackageDTO>> GetAllPackagesAsync()
        {
            var packages = await _context.Packages
                .Include(p => p.PackageCities)
                    .ThenInclude(pc => pc.City)
                    .ThenInclude(c => c.State)
                    .ThenInclude(s => s.Country)
                .Include(p => p.PackageResorts)
                    .ThenInclude(pr => pr.Resort)
                    .ThenInclude(r => r.City)
                    .ThenInclude(c => c.State)
                    .ThenInclude(s => s.Country)
                .Where(p => p.Status == true) // Filter only active packages
                .ToListAsync();

            var packageDTOs = packages.Select(package => new PackageDTO
            {
                PackageId = package.PackageId,
                PackageName = package.PackageName,
                Description = package.Description,
                Price = package.Price,
                CreatedOn = package.CreatedOn,  // Map CreatedOn
                Status = package.Status,        // Map Status

                PackageCities = package.PackageCities.Select(pc => new PackageCityDTO
                {
                    CityId = pc.City.CityId,
                    Description = pc.Description,
                    City = new CityDTO
                    {
                        CityId = pc.City.CityId,
                        CityName = pc.City.CityName,
                        StateName = pc.City.State.StateName,
                        CountryName = pc.City.State.Country.CountryName
                    }
                }).ToList(),

                PackageResorts = package.PackageResorts.Select(pr => new PackageResortDTO
                {
                    ResortId = pr.Resort.ResortId,
                    Description = pr.Description,
                    Resort = new ResortDTO
                    {
                        ResortId = pr.Resort.ResortId,
                        ResortName = pr.Resort.ResortName,
                        CityName = pr.Resort.City.CityName,
                        StateName = pr.Resort.City.State.StateName,
                        CountryName = pr.Resort.City.State.Country.CountryName
                    }
                }).ToList()
            }).ToList();

            return packageDTOs;
        }


        public async Task<PackageDTO> GetPackageByIdAsync(int packageId)
        {
            var package = await _context.Packages
                .Include(p => p.PackageCities)
                    .ThenInclude(pc => pc.City)
                    .ThenInclude(c => c.State)
                    .ThenInclude(s => s.Country)
                .Include(p => p.PackageResorts)
                    .ThenInclude(pr => pr.Resort)
                    .ThenInclude(r => r.City)
                    .ThenInclude(c => c.State)
                    .ThenInclude(s => s.Country)
                .FirstOrDefaultAsync(p => p.PackageId == packageId && p.Status == true); // Include status condition

            if (package == null)
            {
                return null;
            }

            var packageDTO = new PackageDTO
            {
                PackageId = package.PackageId,
                PackageName = package.PackageName,
                Description = package.Description,
                Price = package.Price,
                CreatedOn = package.CreatedOn,  // Include CreatedOn
                Status = package.Status,        // Include Status

                PackageCities = package.PackageCities.Select(pc => new PackageCityDTO
                {
                    CityId = pc.City.CityId,
                    Description = pc.Description,
                    City = new CityDTO
                    {
                        CityId = pc.City.CityId,
                        CityName = pc.City.CityName,
                        StateName = pc.City.State.StateName,
                        CountryName = pc.City.State.Country.CountryName
                    }
                }).ToList(),

                PackageResorts = package.PackageResorts.Select(pr => new PackageResortDTO
                {
                    ResortId = pr.Resort.ResortId,
                    Description = pr.Description,
                    Resort = new ResortDTO
                    {
                        ResortId = pr.Resort.ResortId,
                        ResortName = pr.Resort.ResortName,
                        CityName = pr.Resort.City.CityName,
                        StateName = pr.Resort.City.State.StateName,
                        CountryName = pr.Resort.City.State.Country.CountryName
                    }
                }).ToList()
            };

            return packageDTO;
        }


        // public async Task<Package> CreatePackageAsync(PostPackageDTO packageDto)
        // {
        //     if (packageDto == null)
        //     {
        //         throw new Exception("Package DTO cannot be null.");
        //     }

        //     // Check for existing package with the same details
        //     var existingPackage = await _context.Packages
        //         .Include(p => p.PackageCities)
        //         .Include(p => p.PackageResorts)
        //         .FirstOrDefaultAsync(p => p.PackageName == packageDto.PackageName &&
        //                                    p.Description == packageDto.Description &&
        //                                    p.Price == packageDto.Price);

        //     if (existingPackage != null)
        //     {
        //         // Check if all associated city names match
        //         var existingCityNames = existingPackage.PackageCities.Select(pc => pc.CityId).ToList();
        //         var newCityIds = new List<int>();

        //         foreach (var cityDto in packageDto.PackageCities)
        //         {
        //             var country = await _context.Countries
        //                 .AsNoTracking()
        //                 .FirstOrDefaultAsync(c => c.CountryName == cityDto.CountryName);

        //             if (country == null)
        //             {
        //                 continue; // Country doesn't exist, skip to the next city
        //             }

        //             var state = await _context.States
        //                 .AsNoTracking()
        //                 .FirstOrDefaultAsync(s => s.StateName == cityDto.StateName && s.CountryId == country.CountryId);

        //             if (state == null)
        //             {
        //                 continue; // State doesn't exist, skip to the next city
        //             }

        //             var city = await _context.Cities
        //                 .AsNoTracking()
        //                 .FirstOrDefaultAsync(c => c.CityName == cityDto.CityName && c.StateId == state.StateId);

        //             if (city != null)
        //             {
        //                 newCityIds.Add(city.CityId);
        //             }
        //         }

        //         // Check if all existing cities are in the new cities list
        //         if (existingCityNames.All(id => newCityIds.Contains(id)))
        //         {
        //             throw new Exception("Duplicate packages are not allowed.");
        //         }

        //         // Check for existing resorts
        //         var existingResortIds = existingPackage.PackageResorts.Select(pr => pr.ResortId).ToList();
        //         var newResortIds = new List<int>();

        //         foreach (var resortDto in packageDto.PackageResorts)
        //         {
        //             var resortCity = await _context.Cities
        //                 .AsNoTracking()
        //                 .FirstOrDefaultAsync(c => c.CityName == resortDto.CityName);

        //             if (resortCity == null)
        //             {
        //                 continue; // City doesn't exist, skip to the next resort
        //             }

        //             var resort = await _context.Resorts
        //                 .AsNoTracking()
        //                 .FirstOrDefaultAsync(r => r.ResortName == resortDto.ResortName && r.CityId == resortCity.CityId);

        //             if (resort != null)
        //             {
        //                 newResortIds.Add(resort.ResortId);
        //             }
        //         }

        //         // Check if all existing resorts are in the new resorts list
        //         if (existingResortIds.All(id => newResortIds.Contains(id)))
        //         {
        //             throw new Exception("Duplicate packages are not allowed.");
        //         }
        //     }

        //     // If we reach this point, it means we can create a new package
        //     var package = new Package
        //     {
        //         PackageName = packageDto.PackageName,
        //         Description = packageDto.Description,
        //         Price = packageDto.Price,
        //         CreatedOn = DateTime.UtcNow,  // Set the CreatedOn field
        //         Status = true,
        //         PackageCities = new List<PackageCity>(),
        //         PackageResorts = new List<PackageResort>()
        //     };

        //     // Add Package Cities
        //     foreach (var cityDto in packageDto.PackageCities)
        //     {
        //         if (cityDto == null)
        //         {
        //             throw new Exception("City DTO cannot be null.");
        //         }

        //         if (string.IsNullOrEmpty(cityDto.CityName) || string.IsNullOrEmpty(cityDto.StateName) || string.IsNullOrEmpty(cityDto.CountryName))
        //         {
        //             throw new Exception("City, State, and Country names must be provided.");
        //         }

        //         var country = await _context.Countries
        //             .AsNoTracking()
        //             .FirstOrDefaultAsync(c => c.CountryName == cityDto.CountryName);

        //         if (country == null)
        //         {
        //             country = new Country { CountryName = cityDto.CountryName };
        //             _context.Countries.Add(country);
        //             await _context.SaveChangesAsync();
        //         }

        //         var state = await _context.States
        //             .AsNoTracking()
        //             .FirstOrDefaultAsync(s => s.StateName == cityDto.StateName && s.CountryId == country.CountryId);

        //         if (state == null)
        //         {
        //             state = new State
        //             {
        //                 StateName = cityDto.StateName,
        //                 CountryId = country.CountryId
        //             };
        //             _context.States.Add(state);
        //             await _context.SaveChangesAsync();
        //         }
        //         else
        //         {
        //             if (state.CountryId != country.CountryId)
        //             {
        //                 throw new Exception($"State '{cityDto.StateName}' already exists in another country.");
        //             }
        //         }

        //         var city = await _context.Cities
        //             .AsNoTracking()
        //             .FirstOrDefaultAsync(c => c.CityName == cityDto.CityName && c.StateId == state.StateId);

        //         if (city == null)
        //         {
        //             city = new City
        //             {
        //                 CityName = cityDto.CityName,
        //                 StateId = state.StateId
        //             };
        //             _context.Cities.Add(city);
        //             await _context.SaveChangesAsync();
        //         }
        //         else
        //         {
        //             if (city.StateId != state.StateId)
        //             {
        //                 throw new Exception($"City '{cityDto.CityName}' already exists in another state.");
        //             }
        //         }

        //         package.PackageCities.Add(new PackageCity
        //         {
        //             CityId = city.CityId,
        //             Description = cityDto.CityName
        //         });
        //     }

        //     // Add Package Resorts
        //     foreach (var resortDto in packageDto.PackageResorts)
        //     {
        //         var resortCity = await _context.Cities
        //             .AsNoTracking()
        //             .FirstOrDefaultAsync(c => c.CityName == resortDto.CityName);

        //         if (resortCity == null)
        //         {
        //             throw new Exception($"City '{resortDto.CityName}' not found for the resort.");
        //         }

        //         var resort = await _context.Resorts
        //             .AsNoTracking()
        //             .FirstOrDefaultAsync(r => r.ResortName == resortDto.ResortName && r.CityId == resortCity.CityId);

        //         if (resort == null)
        //         {
        //             resort = new Resort
        //             {
        //                 ResortName = resortDto.ResortName,
        //                 CityId = resortCity.CityId
        //             };
        //             _context.Resorts.Add(resort);
        //             await _context.SaveChangesAsync();
        //         }

        //         package.PackageResorts.Add(new PackageResort
        //         {
        //             ResortId = resort.ResortId,
        //             Description = resortDto.ResortName
        //         });
        //     }

        //     _context.Packages.Add(package);
        //     await _context.SaveChangesAsync();

        //     return package;
        // }

        public async Task<Package> CreatePackageAsync(PostPackageDTO packageDto)
        {
            if (packageDto == null)
            {
                throw new Exception("Package DTO cannot be null.");
            }

            // Check for existing package with the same details
            var existingPackage = await _context.Packages
                .Where(p => p.PackageName == packageDto.PackageName &&
                            p.Description == packageDto.Description &&
                            p.Price == packageDto.Price)
                .Select(p => new
                {
                    Package = p,
                    PackageCities = p.PackageCities.Select(pc => pc.CityId).ToList(),
                    PackageResorts = p.PackageResorts.Select(pr => pr.ResortId).ToList()
                })
                .FirstOrDefaultAsync();

            if (existingPackage != null)
            {
                // Check if all associated city names match
                var existingCityIds = existingPackage.PackageCities;
                var newCityIds = new List<int>();

                foreach (var cityDto in packageDto.PackageCities)
                {
                    var country = await _context.Countries
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.CountryName == cityDto.CountryName);

                    if (country == null)
                    {
                        continue; // Country doesn't exist, skip to the next city
                    }

                    var state = await _context.States
                        .AsNoTracking()
                        .FirstOrDefaultAsync(s => s.StateName == cityDto.StateName && s.CountryId == country.CountryId);

                    if (state == null)
                    {
                        continue; // State doesn't exist, skip to the next city
                    }

                    var city = await _context.Cities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.CityName == cityDto.CityName && c.StateId == state.StateId);

                    if (city != null)
                    {
                        newCityIds.Add(city.CityId);
                    }
                }

                // Check if all existing cities are in the new cities list
                if (existingCityIds.All(id => newCityIds.Contains(id)))
                {
                    throw new Exception("Duplicate packages are not allowed.");
                }

                // Check for existing resorts
                var existingResortIds = existingPackage.PackageResorts;
                var newResortIds = new List<int>();

                foreach (var resortDto in packageDto.PackageResorts)
                {
                    var resortCity = await _context.Cities
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.CityName == resortDto.CityName);

                    if (resortCity == null)
                    {
                        continue; // City doesn't exist, skip to the next resort
                    }

                    var resort = await _context.Resorts
                        .AsNoTracking()
                        .FirstOrDefaultAsync(r => r.ResortName == resortDto.ResortName && r.CityId == resortCity.CityId);

                    if (resort != null)
                    {
                        newResortIds.Add(resort.ResortId);
                    }
                }

                // Check if all existing resorts are in the new resorts list
                if (existingResortIds.All(id => newResortIds.Contains(id)))
                {
                    throw new Exception("Duplicate packages are not allowed.");
                }
            }

            // If we reach this point, it means we can create a new package
            var package = new Package
            {
                PackageName = packageDto.PackageName,
                Description = packageDto.Description,
                Price = packageDto.Price,
                CreatedOn = DateTime.UtcNow,  // Set the CreatedOn field
                Status = true,
                PackageCities = new List<PackageCity>(),
                PackageResorts = new List<PackageResort>()
            };

            // Add Package Cities
            foreach (var cityDto in packageDto.PackageCities)
            {
                if (cityDto == null)
                {
                    throw new Exception("City DTO cannot be null.");
                }

                if (string.IsNullOrEmpty(cityDto.CityName) || string.IsNullOrEmpty(cityDto.StateName) || string.IsNullOrEmpty(cityDto.CountryName))
                {
                    throw new Exception("City, State, and Country names must be provided.");
                }

                var country = await _context.Countries
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.CountryName == cityDto.CountryName);

                if (country == null)
                {
                    country = new Country { CountryName = cityDto.CountryName };
                    _context.Countries.Add(country);
                    await _context.SaveChangesAsync();
                }

                var state = await _context.States
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.StateName == cityDto.StateName && s.CountryId == country.CountryId);

                if (state == null)
                {
                    state = new State
                    {
                        StateName = cityDto.StateName,
                        CountryId = country.CountryId
                    };
                    _context.States.Add(state);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    if (state.CountryId != country.CountryId)
                    {
                        throw new Exception($"State '{cityDto.StateName}' already exists in another country.");
                    }
                }

                var city = await _context.Cities
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.CityName == cityDto.CityName && c.StateId == state.StateId);

                if (city == null)
                {
                    city = new City
                    {
                        CityName = cityDto.CityName,
                        StateId = state.StateId
                    };
                    _context.Cities.Add(city);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    if (city.StateId != state.StateId)
                    {
                        throw new Exception($"City '{cityDto.CityName}' already exists in another state.");
                    }
                }

                package.PackageCities.Add(new PackageCity
                {
                    CityId = city.CityId,
                    Description = cityDto.CityName
                });
            }

            // Add Package Resorts
            foreach (var resortDto in packageDto.PackageResorts)
            {
                var resortCity = await _context.Cities
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.CityName == resortDto.CityName);

                if (resortCity == null)
                {
                    throw new Exception($"City '{resortDto.CityName}' not found for the resort.");
                }

                var resort = await _context.Resorts
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.ResortName == resortDto.ResortName && r.CityId == resortCity.CityId);

                if (resort == null)
                {
                    resort = new Resort
                    {
                        ResortName = resortDto.ResortName,
                        CityId = resortCity.CityId
                    };
                    _context.Resorts.Add(resort);
                    await _context.SaveChangesAsync();
                }

                package.PackageResorts.Add(new PackageResort
                {
                    ResortId = resort.ResortId,
                    Description = resortDto.ResortName
                });
            }

            _context.Packages.Add(package);
            await _context.SaveChangesAsync();

            return package;
        }


        public async Task<Package> UpdatePackageAsync(int packageId, PostPackageDTO packageDto)
        {
            if (packageDto == null)
            {
                throw new Exception("Package DTO cannot be null.");
            }

            // Find the existing package
            var existingPackage = await _context.Packages
                .Include(p => p.PackageCities)
                .Include(p => p.PackageResorts)
                .FirstOrDefaultAsync(p => p.PackageId == packageId);

            if (existingPackage == null)
            {
                throw new Exception($"Package with ID {packageId} not found.");
            }

            // Update package properties
            existingPackage.PackageName = packageDto.PackageName;
            existingPackage.Description = packageDto.Description;
            existingPackage.Price = packageDto.Price;

            // Clear existing cities and resorts for this package only
            existingPackage.PackageCities.Clear();
            existingPackage.PackageResorts.Clear();

            // Add or update Package Cities
            foreach (var cityDto in packageDto.PackageCities)
            {
                if (string.IsNullOrEmpty(cityDto.CityName) || string.IsNullOrEmpty(cityDto.StateName) || string.IsNullOrEmpty(cityDto.CountryName))
                {
                    throw new Exception("City, State, and Country names must be provided.");
                }

                // Ensure the country exists, if not, create it
                var country = await _context.Countries
                    .FirstOrDefaultAsync(c => c.CountryName == cityDto.CountryName);

                if (country == null)
                {
                    country = new Country { CountryName = cityDto.CountryName };
                    _context.Countries.Add(country);
                    await _context.SaveChangesAsync();
                }

                // Ensure the state exists and belongs to the correct country
                var state = await _context.States
                    .FirstOrDefaultAsync(s => s.StateName == cityDto.StateName && s.CountryId == country.CountryId);

                if (state == null)
                {
                    // Create new state if it doesn't exist
                    state = new State
                    {
                        StateName = cityDto.StateName,
                        CountryId = country.CountryId
                    };
                    _context.States.Add(state);
                    await _context.SaveChangesAsync();
                }
                else if (state.CountryId != country.CountryId)
                {
                    throw new Exception($"State '{cityDto.StateName}' is already associated with another country.");
                }

                // Ensure the city exists and belongs to the correct state
                var city = await _context.Cities
                    .FirstOrDefaultAsync(c => c.CityName == cityDto.CityName && c.StateId == state.StateId);

                if (city == null)
                {
                    // Create new city if it doesn't exist
                    city = new City
                    {
                        CityName = cityDto.CityName,
                        StateId = state.StateId
                    };
                    _context.Cities.Add(city);
                    await _context.SaveChangesAsync();
                }
                else if (city.StateId != state.StateId)
                {
                    throw new Exception($"City '{cityDto.CityName}' is already associated with another state.");
                }

                // Add the city to the package
                existingPackage.PackageCities.Add(new PackageCity
                {
                    CityId = city.CityId,
                    Description = cityDto.CityName
                });
            }

            // Add or update Package Resorts
            foreach (var resortDto in packageDto.PackageResorts)
            {
                var resortCity = await _context.Cities
                    .FirstOrDefaultAsync(c => c.CityName == resortDto.CityName);

                if (resortCity == null)
                {
                    throw new Exception($"City '{resortDto.CityName}' not found for the resort.");
                }

                var resort = await _context.Resorts
                    .FirstOrDefaultAsync(r => r.ResortName == resortDto.ResortName && r.CityId == resortCity.CityId);

                if (resort == null)
                {
                    // Create new resort if it doesn't exist
                    resort = new Resort
                    {
                        ResortName = resortDto.ResortName,
                        CityId = resortCity.CityId
                    };
                    _context.Resorts.Add(resort);
                    await _context.SaveChangesAsync();
                }

                // Add the resort to the package
                existingPackage.PackageResorts.Add(new PackageResort
                {
                    ResortId = resort.ResortId,
                    Description = resortDto.ResortName
                });
            }

            // Update the existing package
            _context.Packages.Update(existingPackage);
            await _context.SaveChangesAsync();

            return existingPackage;
        }


        public async Task<bool> DeletePackageAsync(int packageId)
        {
            var package = await _context.Packages
                .FirstOrDefaultAsync(p => p.PackageId == packageId);

            if (package == null)
            {
                throw new Exception("Package not found.");
            }

            // Set status to false (soft delete)
            package.Status = false;

            _context.Packages.Update(package);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
