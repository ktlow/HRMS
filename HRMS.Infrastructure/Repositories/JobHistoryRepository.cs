using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRMS.Core.Entities;
using HRMS.Core.Interfaces;
using HRMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories
{
    public class JobHistoryRepository : IJobHistoryRepository
    {
        private readonly HRMSDbContext _context;

        public JobHistoryRepository(HRMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobHistory>> GetAllAsync()
        {

            return await _context.JobHistories
                .Include(j => j.Employee)
                .Include(j => j.Manager)
                .Include(j => j.JobRole)
                .ToListAsync();
        }

        public async Task<JobHistory> GetByIdAsync(int id)
        {
            return await _context.JobHistories.FindAsync(id);
        }

        public async Task AddAsync(JobHistory jobhistory)
        {
            // Retrieve the existing employee from the database
            var employee = await _context.Employees.FindAsync(jobhistory.EmployeeId);

            if (employee != null)
            {  // Assign the location entity to avoid EF trying to insert it again
                jobhistory.Employee = employee;
            }
            var manager = await _context.Employees.FindAsync(jobhistory.ManagerId);

            if (manager != null)
            {  // Assign the location entity to avoid EF trying to insert it again
                jobhistory.Manager = manager;
            }
            else
            {
                jobhistory.ManagerId = null;
            }
                // Retrieve the existing jobRole from the database
                var jobRole = await _context.JobRoles.FindAsync(jobhistory.JobRoleId);

            if (jobRole != null)
            {  // Assign the location entity to avoid EF trying to insert it again
                jobhistory.JobRole = jobRole;
            }
            else
            {
                jobhistory.JobRole = null;
            }

            _context.JobHistories.Add(jobhistory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobHistory jobhistory)
        {
            _context.JobHistories.Update(jobhistory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(JobHistory jobhistory)
        {
            _context.JobHistories.Remove(jobhistory);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<JobHistory>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.JobHistories
                    .Include(jh => jh.Employee)
                    .Include(jh => jh.Manager)
                    .Include(jh => jh.JobRole)
                    .Where(jh => jh.EmployeeId == employeeId)
                    .ToListAsync();

        }
        public async Task<JobHistory> AddJobHistory(int employeeId, JobHistory jobHistory)
        {
            // Ensure the employee exists
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
            {
                throw new ArgumentException($"Employee with ID {employeeId} not found.");
            }

            // Set the EmployeeId for the job history
            jobHistory.EmployeeId = employeeId;

            // Add job history to the database
            _context.JobHistories.Add(jobHistory);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return the created job history record
            return jobHistory;
        }

    }
}