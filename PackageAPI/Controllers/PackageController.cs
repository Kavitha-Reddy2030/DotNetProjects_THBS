using Microsoft.AspNetCore.Mvc;
using PackageAPI.Models;
using PackageAPI.BusinessLayer;
using System.Collections.Generic;
using System.Threading.Tasks;
using PackageAPI.DTO;
using PackageAPI.DTOs;

namespace PackageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;

        public PackageController(IPackageService packageService)
        {
            _packageService = packageService;
        }
        

        // GET: api/Package
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PackageDTO>>> GetPackages()
        {
            var packages = await _packageService.GetAllPackagesAsync();
            return Ok(packages);
        }


        // GET: api/Package/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Package>> GetPackage(int id)
        {
            var package = await _packageService.GetPackageByIdAsync(id);

            if (package == null)
            {
                return NotFound();
            }

            return Ok(package);
        }


        [HttpPost]
        public async Task<IActionResult> CreatePackage([FromBody] PostPackageDTO packageDto)
        {
            if (packageDto == null)
            {
                return BadRequest("Package data is null.");
            }

            try
            {
                var createdPackage = await _packageService.CreatePackageAsync(packageDto);

                // Create the response DTO to return
                var response = new PackageResponseDTO
                {
                    PackageId = createdPackage.PackageId,
                    PackageName = createdPackage.PackageName,
                    Description = createdPackage.Description,
                    Price = createdPackage.Price,
                    PackageCities = createdPackage.PackageCities.Select(pc => new CityResponseDTO
                    {
                        CityId = pc.City.CityId,
                        CityName = pc.City.CityName,
                        Description = pc.Description
                    }).ToList(),
                    PackageResorts = createdPackage.PackageResorts.Select(pr => new ResortResponseDTO
                    {
                        ResortId = pr.Resort.ResortId,
                        ResortName = pr.Resort.ResortName,
                        Description = pr.Description
                    }).ToList()
                };

                return CreatedAtAction(nameof(CreatePackage), new { id = createdPackage.PackageId }, new { message = "Data inserted successfully", data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{packageId}")]
        public async Task<IActionResult> UpdatePackage(int packageId, [FromBody] PostPackageDTO packageDto)
        {
            if (packageDto == null)
            {
                return BadRequest("Package data is null");
            }

            try
            {
                var updatedPackage = await _packageService.UpdatePackageAsync(packageId, packageDto);

                if (updatedPackage == null)
                {
                    return NotFound($"Package with ID {packageId} was not found.");
                }

                return Ok(updatedPackage);
            }
            catch (Exception ex)
            {
                // Log the exception (you can use any logging framework here)
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating the package.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePackage(int id)
        {
            try
            {
                var result = await _packageService.DeletePackageAsync(id);
                if (result)
                {
                    return NoContent(); // 204 No Content on successful deletion
                }
                return NotFound(new { message = "Package not found." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
