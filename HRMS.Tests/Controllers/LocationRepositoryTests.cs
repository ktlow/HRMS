using HRMS.Core.Entities;
using HRMS.Infrastructure.Data;
using HRMS.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Linq;

namespace HRMS.Tests.Repositories
{
    public class LocationRepositoryTests
    {
        private readonly HRMSDbContext _context;
        private readonly LocationRepository _repository;

        public LocationRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<HRMSDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new HRMSDbContext(options);
            _repository = new LocationRepository(_context);
        }

        [Fact]
        public async Task AddAsync_Should_Add_Location()
        {
            var location = new Location { City = "New York", Name = "Office in New York" };

            await _repository.AddAsync(location);
            var locations = await _repository.GetAllAsync();

            Assert.Single(locations);
            Assert.Equal("New York", locations.First().City);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Correct_Location()
        {
            var location = new Location { City = "London", Name = "Office in London" };
            await _repository.AddAsync(location);

            var result = await _repository.GetByIdAsync(location.Id);

            Assert.NotNull(result);
            Assert.Equal("London", result.City);
        }

        [Fact]
        public async Task UpdateAsync_Should_Modify_Location()
        {
            var location = new Location {City = "San Francisco", Name = "Office in SF" };
            await _repository.AddAsync(location);

            location.City = "Los Angeles";
            location.Name = "Office in LA";
            await _repository.UpdateAsync(location);

            var updatedLocation = await _repository.GetByIdAsync(location.Id);
            Assert.Equal("Los Angeles", updatedLocation.City);
            Assert.Equal("Office in LA", updatedLocation.Name);
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Location()
        {
            var location = new Location {City = "Chicago", Name = "Office in Chicago" };
            await _repository.AddAsync(location);

            await _repository.DeleteAsync(location);
            var allLocations = await _repository.GetAllAsync();

            Assert.Empty(allLocations);
        }
    }
}
