using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HRMS.Core.Entities;  // Core entities
using HRMS.Core.Interfaces;  // Interfaces for repositories
using HRMS.API.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;  // Dto

namespace HRMS.API.Controllers
{
    [Route("api/JobRole")]
    [ApiController]
    public class JobRoleController : ControllerBase
    {
        private readonly IJobRoleRepository _jobroleRepository;

        public JobRoleController(IJobRoleRepository jobroleRepository)
        {
            _jobroleRepository = jobroleRepository;
        }

        [HttpPost]
        public async Task<ActionResult<JobRole>> CreateJobRole(JobRoleDto jobrole)
        {
            try
            {
            var jobRole = new JobRole
            {
                Title = jobrole.Title,
                Description = jobrole.Description,
                DepartmentId = jobrole.DepartmentId
            };
            await _jobroleRepository.AddAsync(jobRole);
            return CreatedAtAction(nameof(GetJobRole), new { id = jobrole.Id }, jobrole);

            } 
            catch (DbUpdateException dbEx)
            {
                return BadRequest(new { message = "Database update failed. Check foreign keys and data integrity.", details = dbEx.Message});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobRoleDto>>> GetJobRoles()
        {
            var jobRoles = await _jobroleRepository.GetAllAsync();

            var result = jobRoles.Select(static d => new JobRoleDto
            {
                Id = d.Id,
                DepartmentId = d.DepartmentId,
                Description = d.Description,
                Title=d.Title,
                Department = d.Department == null ? null : new DepartmentDto
                {
                    Id = d.Department.Id,
                    Description = d.Department.Description,
                    Name = d.Department.Name,
                    LocationId = d.Department.LocationId,

                    Location = d.Department.Location == null ? null : new LocationDto
                    {
                        Id = d.Department.Location.Id,
                        City = d.Department.Location?.City,
                        Country = d.Department.Location?.Country
                    }
                }
            });

            return Ok(result);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JobRole>> GetJobRole(int id)
        {
            var jobrole = await _jobroleRepository.GetByIdAsync(id);
            if (jobrole == null)
            {
                return NotFound();
            }
            return Ok(jobrole);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJobRole(int id, JobRole jobrole)
        {
            if (id != jobrole.Id)
            {
                return BadRequest();
            }

            await _jobroleRepository.UpdateAsync(jobrole);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobRole(int id)
        {
            var jobrole = await _jobroleRepository.GetByIdAsync(id);
            if (jobrole == null)
            {
                return NotFound();
            }

            await _jobroleRepository.DeleteAsync(jobrole);
            return NoContent();
        }
    }
}
