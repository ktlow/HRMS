using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HRMS.Core.Entities;  // Core entities
using HRMS.Core.Interfaces;
using Microsoft.EntityFrameworkCore;  // Interfaces for repositories

namespace HRMS.API.Controllers
{
    [Route("api/Location")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepository _locationRepository;

        public LocationController(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        [HttpPost]
        public async Task<ActionResult<Location>> CreateLocation(Location location)
        {
            try
            {
                await _locationRepository.AddAsync(location);
                return CreatedAtAction(nameof(GetLocation), new { id = location.Id }, location);

            }
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new { message = "Database update failed. Check foreign keys and data integrity.", details = dbEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
        {
            var locations = await _locationRepository.GetAllAsync();
            return Ok(locations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocation(int id)
        {
            var location = await _locationRepository.GetByIdAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            return Ok(location);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocation(int id, Location location)
        {
            if (id != location.Id)
            {
                return BadRequest();
            }

            await _locationRepository.UpdateAsync(location);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var location = await _locationRepository.GetByIdAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            await _locationRepository.DeleteAsync(location);
            return NoContent();
        }
    }
}
