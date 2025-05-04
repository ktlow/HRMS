using HRMS.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using HRMS.Core.Entities;  // Core entities
using HRMS.Core.Interfaces;
using Microsoft.EntityFrameworkCore;  // Interfaces for repositories

namespace HRMS.API.Controllers
{
    [Route("api/Department")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly ILocationRepository _locationRepository;

        public DepartmentController(IDepartmentRepository departmentRepository, ILocationRepository locationRepository)
        {
            _departmentRepository = departmentRepository;
            _locationRepository = locationRepository;
        }

        [HttpPost]
        public async Task<ActionResult<DepartmentDto>> CreateDepartment(DepartmentDto departmentDto)
        {
            try
            {
                var department = new Department
                {
                    Name = departmentDto.Name,
                    Description = departmentDto.Description,
                    LocationId = departmentDto.LocationId
                };

                await _departmentRepository.AddAsync(department);

                // Optional: Load Location if you want to return it in the DTO
                var location = await _locationRepository.GetByIdAsync(department.LocationId); // Or use EF's context if needed

                var resultDto = new DepartmentDto
                {
                    Id = department.Id,
                    Name = department.Name,
                    Description = department.Description,
                    LocationId = department.LocationId,
                    Location = location != null
                        ? new LocationDto
                        {
                            Id = location.Id,
                            Name = location.Name
                        }
                        : null
                };

                return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, resultDto);
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
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            var departments = await _departmentRepository.GetAllAsync();

            var result = departments.Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                LocationId = d.LocationId,
                Location = d.Location == null ? null : new LocationDto
                {
                    Id = d.Location.Id,
                    Name = d.Location.Name,
                    City = d.Location.City,
                    Country = d.Location.Country
                }
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, Department department)
        {
            if (id != department.Id)
            {
                return BadRequest();
            }

            await _departmentRepository.UpdateAsync(department);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                return NotFound();
            }

            await _departmentRepository.DeleteAsync(department);
            return NoContent();
        }
    }

}
