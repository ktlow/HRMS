using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HRMS.Core.Entities;  // Reference to entities from HRMS.Core
namespace HRMS.Infrastructure.Data
{
    public class HRMSDbContext : DbContext
    {
        public HRMSDbContext(DbContextOptions<HRMSDbContext> options) : base(options) { }

        // DbSets for each entity
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<JobHistory> JobHistories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<JobRole> JobRoles { get; set; }

        // Fluent API configurations (optional)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<JobHistory>()
                .HasOne(jh => jh.Employee)
                .WithMany(e => e.JobHistories)  // Assuming Employee has a JobHistories collection
                .HasForeignKey(jh => jh.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<JobHistory>()
                .HasOne(jh => jh.Manager)  // Assuming JobHistory has a Manager property
                .WithMany()  // Managers might not have a collection of JobHistories
                .HasForeignKey(jh => jh.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict); 

        }
    }
}
