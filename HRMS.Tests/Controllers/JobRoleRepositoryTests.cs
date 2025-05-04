using HRMS.Core.Entities;
using HRMS.Infrastructure.Data;
using HRMS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Linq;

namespace HRMS.Tests.Repositories
{
    public class JobRoleRepositoryTests
    {
        private readonly HRMSDbContext _context;
        private readonly JobRoleRepository _repository;

        public JobRoleRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<HRMSDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new HRMSDbContext(options);
            _repository = new JobRoleRepository(_context);
        }

        [Fact]
        public async Task AddAsync_Should_Add_JobRole()
        {   // Arrange: seed a Location first (required by FK)
            var location = new Location { Id = 1, Name = "HQ", City = "London" };
            await _context.Locations.AddAsync(location);
            await _context.SaveChangesAsync();


            // Arrange: seed a Department first (required by FK)
            var department = new Department { Id = 1, Name = "IT", Description = "IT Dept", Location= location, LocationId= location.Id };
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();

            var jobRole = new JobRole { Title = "Software Developer", Description = "Develops software." ,DepartmentId = department.Id, Department = department};

            await _repository.AddAsync(jobRole);
            var jobRoles = await _repository.GetAllAsync();

            Assert.Single(jobRoles);
            Assert.Equal("Software Developer", jobRoles.First().Title);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Correct_JobRole()
        {
            var jobRole = new JobRole { Title = "Project Manager", Description = "Manages projects." };
            await _repository.AddAsync(jobRole);

            var result = await _repository.GetByIdAsync(jobRole.Id);

            Assert.NotNull(result);
            Assert.Equal("Project Manager", result.Title);
        }

        [Fact]
        public async Task UpdateAsync_Should_Modify_JobRole()
        {
            var jobRole = new JobRole { Title = "System Analyst", Description = "Analyzes systems." };
            await _repository.AddAsync(jobRole);

            jobRole.Title = "Business Analyst";
            jobRole.Description = "Analyzes business requirements.";
            await _repository.UpdateAsync(jobRole);

            var updatedJobRole = await _repository.GetByIdAsync(jobRole.Id);
            Assert.Equal("Business Analyst", updatedJobRole.Title);
            Assert.Equal("Analyzes business requirements.", updatedJobRole.Description);
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_JobRole()
        {
            var jobRole = new JobRole { Title = "HR Manager", Description = "Manages human resources." };
            await _repository.AddAsync(jobRole);

            await _repository.DeleteAsync(jobRole);
            var allJobRoles = await _repository.GetAllAsync();

            Assert.Empty(allJobRoles);
        }
    }
}
