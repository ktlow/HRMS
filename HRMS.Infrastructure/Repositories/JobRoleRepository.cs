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
    public class JobRoleRepository : IJobRoleRepository
    {
        private readonly HRMSDbContext _context;

        public JobRoleRepository(HRMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobRole>> GetAllAsync()
        {
            return await _context.JobRoles
                .Include(j => j.Department) 
                .ThenInclude(static d => d.Location)
                .ToListAsync();
        }

        public async Task<JobRole> GetByIdAsync(int id)
        {
            return await _context.JobRoles.FindAsync(id);
        }

        public async Task AddAsync(JobRole jobrole)
        {
            // Retrieve the existing department from the database
            var department = await _context.Departments.FindAsync(jobrole.DepartmentId);

            if (department != null)
            {
                // Assign the department entity to avoid EF trying to insert it again
                jobrole.Department = department;
            }

            _context.JobRoles.Add(jobrole);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(JobRole jobrole)
        {
            _context.JobRoles.Update(jobrole);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(JobRole jobrole)
        {
            _context.JobRoles.Remove(jobrole);
            await _context.SaveChangesAsync();
        }
    }
}
