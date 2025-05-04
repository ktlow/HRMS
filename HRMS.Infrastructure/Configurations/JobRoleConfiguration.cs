using HRMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class JobRoleConfiguration : IEntityTypeConfiguration<JobRole>
{
    public void Configure(EntityTypeBuilder<JobRole> builder)
    {
        builder.HasKey(jr => jr.Id);
        builder.Property(jr => jr.Title).IsRequired().HasMaxLength(100);
        builder.Property(jr => jr.Description).HasMaxLength(250);

        builder.HasOne(jr => jr.Department)
            .WithMany()
            .HasForeignKey(jr => jr.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
