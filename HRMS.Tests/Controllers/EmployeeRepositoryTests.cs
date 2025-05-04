using HRMS.Core.Entities;
using HRMS.Infrastructure.Data;
using HRMS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HRMS.Tests.Repositories
{
    public class EmployeeRepositoryTests
    {
        private readonly HRMSDbContext _context;
        private readonly EmployeeRepository _repository;

        public EmployeeRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<HRMSDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new HRMSDbContext(options);
            _repository = new EmployeeRepository(_context);
        }

        [Fact]
        public async Task AddAsync_Should_Add_Employee()
        {
            var employee = new Employee { Name = "John Doe", Email = "john@example.com" };

            await _repository.AddAsync(employee);
            var employees = await _repository.GetAllAsync();

            Assert.Single(employees);
            Assert.Equal("John Doe", employees.First().Name);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Correct_Employee()
        {
            var employee = new Employee { Name = "Jane Doe", Email = "jane@example.com" };
            await _repository.AddAsync(employee);

            var result = await _repository.GetByIdAsync(employee.Id);

            Assert.NotNull(result);
            Assert.Equal("Jane Doe", result.Name);
        }

        [Fact]
        public async Task UpdateAsync_Should_Modify_Employee()
        {
            var employee = new Employee { Name = "Alice", Email = "alice@example.com" };
            await _repository.AddAsync(employee);

            employee.Name = "Alice Updated";
            await _repository.UpdateAsync(employee);

            var updated = await _repository.GetByIdAsync(employee.Id);
            Assert.Equal("Alice Updated", updated.Name);
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Employee()
        {
            var employee = new Employee { Name = "Bob", Email = "bob@example.com" };
            await _repository.AddAsync(employee);

            await _repository.DeleteAsync(employee);
            var all = await _repository.GetAllAsync();

            Assert.Empty(all);
        }
    }
}
