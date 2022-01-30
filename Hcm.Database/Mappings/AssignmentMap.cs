using Hcm.Database.Common;
using Hcm.Database.Constants;
using Hcm.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hcm.Database.Mappings
{
    public sealed class AssignmentMap : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.ToTable(Tables.Assignments, "dbo");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasValueGenerator<IdValueGenerator>()
                .HasColumnName("id")
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(e => e.JobTitle)
                .HasMaxLength(32)
                .HasColumnName("job_title")
                .IsRequired();

            builder.Property(e => e.Start)
                .HasColumnName("start_date")
                .IsRequired();

            builder.Property(e => e.End)
                .HasColumnName("end_date");

            builder.Property(e => e.EmployeeId)
               .HasColumnName("employee_id")
               .IsRequired();

            builder.Property(e => e.DepartmentId)
               .HasColumnName("department_id")
               .IsRequired();

            builder.HasOne(e => e.Employee)
               .WithMany(e => e.Assignments)
               .HasForeignKey(e => e.EmployeeId);

            builder.HasOne(e => e.Department)
               .WithMany(e => e.Assignments)
               .HasForeignKey(e => e.DepartmentId);

            builder.HasMany(e => e.Sallaries)
              .WithOne(e => e.Assignment)
              .HasForeignKey(e => e.AssignmentId);
        }
    }
}
