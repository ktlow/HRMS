using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HRMS.Core.Entities;  // Core entities
using HRMS.Core.Interfaces;  // Interfaces for repositories
using HRMS.API.Models;
using Microsoft.EntityFrameworkCore;
using HRMS.Infrastructure.Repositories;

namespace HRMS.API.Controllers
{
    [Route("api/JobHistory")]
    [ApiController]
    public class JobHistoryController : ControllerBase
    {
        private readonly IJobHistoryRepository _jobhistoryRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public JobHistoryController(IJobHistoryRepository jobhistoryRepository, IEmployeeRepository employeeRepository)
        {
            _jobhistoryRepository = jobhistoryRepository;
            _employeeRepository = employeeRepository;
        }


        [HttpPost]
        public async Task<ActionResult<JobHistoryDto>> CreateJobHistory(JobHistoryDto jobhistorydto)
        {
            try
            {
                if (jobhistorydto.EndDate.HasValue && jobhistorydto.EndDate < jobhistorydto.StartDate)
                    return BadRequest(new { message = "EndDate cannot be earlier than StartDate." });

                if (jobhistorydto.EmployeeId == null)
                    return BadRequest(new { message = "EmployeeId is required." });

                var employeeExists = await _employeeRepository.ExistsAsync(jobhistorydto.EmployeeId.Value);
                if (!employeeExists)
                    return NotFound(new { message = $"Employee with ID {jobhistorydto.EmployeeId} not found." });

                var jobhistory = new JobHistory
                {
                    EmployeeId = jobhistorydto.EmployeeId,
                    ManagerId = jobhistorydto.ManagerId,
                    JobRoleId = jobhistorydto.JobRoleId,
                    StartDate = jobhistorydto.StartDate,
                    EndDate = jobhistorydto.EndDate,
                    Status = jobhistorydto.Status,
                    Comments = jobhistorydto.Comments
                };

                await _jobhistoryRepository.AddAsync(jobhistory);

                var resultDto = new JobHistoryDto
                {
                    Id = jobhistory.Id,
                    EmployeeId = jobhistory.EmployeeId,
                    ManagerId = jobhistory.ManagerId,
                    JobRoleId = jobhistory.JobRoleId,
                    StartDate = jobhistory.StartDate,
                    EndDate = jobhistory.EndDate,
                    Status = jobhistory.Status,
                    Comments = jobhistory.Comments
                };

                return CreatedAtAction(nameof(GetJobHistory), new { id = jobhistory.Id }, resultDto);
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
        public async Task<ActionResult<IEnumerable<JobHistory>>> GetJobHistorys()
        {
            var jobhistorys = await _jobhistoryRepository.GetAllAsync();
            return Ok(jobhistorys);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JobHistory>> GetJobHistory(int id)
        {
            var jobhistory = await _jobhistoryRepository.GetByIdAsync(id);
            if (jobhistory == null)
            {
                return NotFound();
            }
            return Ok(jobhistory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJobHistory(int id, JobHistory jobhistory)
        {
            if (id != jobhistory.Id)
            {
                return BadRequest();
            }

            await _jobhistoryRepository.UpdateAsync(jobhistory);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobHistory(int id)
        {
            var jobhistory = await _jobhistoryRepository.GetByIdAsync(id);
            if (jobhistory == null)
            {
                return NotFound();
            }

            await _jobhistoryRepository.DeleteAsync(jobhistory);
            return NoContent();
        }
    }
}
