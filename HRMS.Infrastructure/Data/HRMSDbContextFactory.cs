using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HRMS.Infrastructure.Data
{
    public class HRMSDbContextFactory : IDesignTimeDbContextFactory<HRMSDbContext>
    {
        public HRMSDbContext CreateDbContext(string[] args)
        {
            // Path to the API project's appsettings.json
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../HRMS.API");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<HRMSDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new HRMSDbContext(optionsBuilder.Options);
        }
    }
}
