using HRMS.Core.Entities;
using HRMS.Core.Interfaces;
using HRMS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HRMSDbContext _context;

        public EmployeeRepository(HRMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .Include(e=>e.Department)
                .Include(e => e.JobHistories)  // Include the related JobHistories
                .ToListAsync();

        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }
        
        public async Task CreateEmployee(Employee employee)
        {// Retrieve the existing department from the database
            var department = await _context.Departments.FindAsync(employee.DepartmentId);

            if (department != null)
            {
                // Assign the department entity to avoid EF trying to insert it again
                employee.Department = department;
            }

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(int employeeId)
        {
            return await _context.Employees.AnyAsync(e => e.Id == employeeId);
        }

    }

}
