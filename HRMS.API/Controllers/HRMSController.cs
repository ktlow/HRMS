using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HRMS.Core.Entities;  // Core entities
using HRMS.Core.Interfaces;
using Microsoft.EntityFrameworkCore;  // Interfaces for repositories
using HRMS.API.Models;
using HRMS.Infrastructure.Repositories;
using System.Runtime.Intrinsics.Arm;

namespace HRMS.API.Controllers
{
    [Route("api/Employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IJobHistoryRepository _jobHistoryRepository;

        public EmployeeController(IEmployeeRepository employeeRepository, IJobHistoryRepository jobHistoryRepository)
        {
            _employeeRepository = employeeRepository;
            _jobHistoryRepository = jobHistoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployees()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeeDtos = employees.Select(static e => new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                Email = e.Email,
                Status = e.Status,
                Phone=e.Phone,
                Address=e.Address,
                BirthDate=e.BirthDate,
                Gender=e.Gender,
                DepartmentId=e.DepartmentId,
                Department = e.Department == null ? null : new DepartmentDto
                {
                    Id = e.Department.Id,
                    Name = e.Department.Name,
                    Description = e.Department.Description,
                    LocationId = e.Department.LocationId,
                    Location = e.Department.Location == null ? null : new LocationDto
                    {
                        Id = e.Department.Location.Id,
                        Name = e.Department.Location.Name
                    }
                },
                JobHistories = e.JobHistories?.Select(jh => new JobHistoryDto
                {
                    Id = jh.Id,
                    EmployeeId = jh.EmployeeId,
                    ManagerId = jh.ManagerId,
                    JobTitle = jh.JobRole?.Title ?? "", // Default value if JobRole is null
                    StartDate = jh.StartDate,
                    EndDate = jh.EndDate,
                    Status = jh.Status,
                    Comments = jh.Comments
                }).ToList()
            });

            return Ok(employeeDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpGet("{employeeId}/jobhistories")]
        public async Task<ActionResult<IEnumerable<JobHistoryDto>>> GetJobHistoriesByEmployeeId(int employeeId)
        {
            var histories = await _jobHistoryRepository.GetByEmployeeIdAsync(employeeId);

            var result = histories.Select(h => new JobHistoryDto
            {
                Id = h.Id,
                EmployeeId = h.EmployeeId,
                ManagerId = h.ManagerId,
                JobRoleId = h.JobRoleId,
                JobTitle = h.JobRole?.Title ?? "", // if JobRole is included
                EmployeeName = h.Employee?.Name ?? "", // if Employee is included
                ManagerName = h.Manager?.Name ?? "",
                StartDate = h.StartDate,
                EndDate = h.EndDate,
                Status = h.Status,
                Comments = h.Comments
            });

            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee([FromBody] Employee employee)
        {
            try { 
                if (employee == null)
                    return BadRequest();

                await _employeeRepository.AddAsync(employee);
                return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            await _employeeRepository.UpdateAsync(employee);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            await _employeeRepository.DeleteAsync(employee);
            return NoContent();
        }

        [HttpPost("{employeeId}/jobHistory")]
        public async Task<IActionResult> AddJobHistory(int employeeId, [FromBody] JobHistoryDto jobhistorydto)
        {
            try
            {

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

                var newJobHistory = await _jobHistoryRepository.AddJobHistory(employeeId, jobhistory);

                return CreatedAtAction(nameof(JobHistoryController.GetJobHistorys), new { id = newJobHistory.Id }, newJobHistory);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding the job history.", details = ex.Message });
            }
        }

    }

}
