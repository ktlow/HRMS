using HRMS.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class JobHistoryConfiguration : IEntityTypeConfiguration<JobHistory>
{
    public void Configure(EntityTypeBuilder<JobHistory> builder)
    {
        builder.HasKey(jh => jh.Id);
        builder.Property(jh => jh.StartDate).IsRequired();
        builder.Property(jh => jh.EndDate);
        builder.Property(jh => jh.Status).IsRequired();
        builder.Property(jh => jh.Comments).HasMaxLength(500);

        builder.HasOne(jh => jh.Employee)
            .WithMany()
            .HasForeignKey(jh => jh.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(jh => jh.JobRole)
            .WithMany()
            .HasForeignKey(jh => jh.JobRoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(jh => jh.Manager)
            .WithMany()
            .HasForeignKey(jh => jh.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
