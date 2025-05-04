using HRMS.Core.Entities;
using HRMS.Infrastructure.Data;
using HRMS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HRMS.Tests
{
    public class JobHistoryRepositoryTests
    {
        private async Task<HRMSDbContext> GetInMemoryDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<HRMSDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new HRMSDbContext(options);
            await context.Database.EnsureCreatedAsync();
            return context;
        }

        [Fact]
        public async Task AddAsync_Should_Add_JobHistory()
        {
            // Arrange
            var context = await GetInMemoryDbContextAsync();
            var repository = new JobHistoryRepository(context);
            var jobHistory = new JobHistory
            {
                EmployeeId = 1,
                JobRoleId = 1,
                StartDate = DateTime.UtcNow,
                Status = "Active",
                Comments = "First entry"
            };

            // Act
            await repository.AddAsync(jobHistory);

            // Assert
            var saved = await context.JobHistories.FindAsync(jobHistory.Id);
            Assert.NotNull(saved);
            Assert.Equal("First entry", saved!.Comments);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_JobHistories()
        {
            var options = new DbContextOptionsBuilder<HRMSDbContext>()
                        .UseInMemoryDatabase(databaseName: "JobHistoryTestDb")
                        .Options;

            using (var context = new HRMSDbContext(options))
            {
                // Seed required dependencies
                context.Employees.AddRange(
                    new Employee { Id = 1, Name = "Alice" },
                    new Employee { Id = 2, Name = "Bob" }
                );

                context.JobRoles.AddRange(
                    new JobRole { Id = 1, Title = "Developer" },
                    new JobRole { Id = 2, Title = "Manager" }
                );

                // Now seed JobHistories
                context.JobHistories.AddRange(
                    new JobHistory { EmployeeId = 1, JobRoleId = 1, StartDate = DateTime.Now, Status = "Active" },
                    new JobHistory { EmployeeId = 2, JobRoleId = 2, StartDate = DateTime.Now, Status = "Inactive" }
                );

                context.SaveChanges();
            }

        }

        [Fact]
        public async Task UpdateAsync_Should_Modify_Existing_JobHistory()
        {
            // Arrange
            var context = await GetInMemoryDbContextAsync();
            var jobHistory = new JobHistory
            {
                EmployeeId = 1,
                JobRoleId = 1,
                StartDate = DateTime.Now,
                Status = "Active",
                Comments = "Initial"
            };
            context.JobHistories.Add(jobHistory);
            await context.SaveChangesAsync();

            var repository = new JobHistoryRepository(context);

            // Act
            jobHistory.Comments = "Updated";
            jobHistory.Status = "Inactive";
            await repository.UpdateAsync(jobHistory);

            // Assert
            var updated = await context.JobHistories.FindAsync(jobHistory.Id);
            Assert.NotNull(updated);
            Assert.Equal("Updated", updated!.Comments);
            Assert.Equal("Inactive", updated.Status);
        }
    }
}
