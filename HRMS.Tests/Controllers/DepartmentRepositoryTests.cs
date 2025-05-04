using HRMS.Core.Entities;
using HRMS.Infrastructure.Data;
using HRMS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Linq;

namespace HRMS.Tests.Repositories
{
    public class DepartmentRepositoryTests
    {
        private readonly HRMSDbContext _context;
        private readonly DepartmentRepository _repository;

        public DepartmentRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<HRMSDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new HRMSDbContext(options);
            _repository = new DepartmentRepository(_context);
        }

        [Fact]
        public async Task AddAsync_Should_Add_Department()
        {
            // Arrange: seed a Location first (required by FK)
            var location = new Location { Id = 1, Name = "HQ", City = "London" };
            await _context.Locations.AddAsync(location);
            await _context.SaveChangesAsync();

            var department = new Department
            {
                Name = "HR",
                Description = "Human Resources",
                LocationId = location.Id
            };

            // Act
            await _repository.AddAsync(department);

            // Assert
            var departments = await _repository.GetAllAsync();
            Assert.Single(departments);
            Assert.Equal("HR", departments.First().Name);
            Assert.Equal("HQ", departments.First().Location?.Name);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Correct_Department()
        {
            var department = new Department { Name = "IT", Description = "Information Technology" };
            await _repository.AddAsync(department);

            var result = await _repository.GetByIdAsync(department.Id);

            Assert.NotNull(result);
            Assert.Equal("IT", result.Name);
        }

        [Fact]
        public async Task UpdateAsync_Should_Modify_Department()
        {
            var department = new Department { Name = "Sales", Description = "Sales Department" };
            await _repository.AddAsync(department);

            department.Name = "Sales & Marketing";
            await _repository.UpdateAsync(department);

            var updated = await _repository.GetByIdAsync(department.Id);
            Assert.Equal("Sales & Marketing", updated.Name);
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Department()
        {
            var department = new Department { Name = "Finance", Description = "Finance Department" };
            await _repository.AddAsync(department);

            await _repository.DeleteAsync(department);
            var all = await _repository.GetAllAsync();

            Assert.Empty(all);
        }
    }
}
