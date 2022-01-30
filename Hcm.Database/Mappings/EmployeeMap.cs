using Hcm.Database.Common;
using Hcm.Database.Constants;
using Hcm.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hcm.Database.Mappings
{
    public class EmployeeMap : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable(Tables.Employees, "dbo");

            builder.HasIndex(e => e.Email)
                .HasName("IX_employees_email")
                .IsUnique();

            builder.HasIndex(e => e.Phone)
                .HasName("IX_employees_phone")
                .IsUnique();

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasValueGenerator<IdValueGenerator>()
                .HasColumnName("id")
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(e => e.UserId)
                .HasMaxLength(64)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(e => e.FirstName)
                .HasMaxLength(64)
                .HasColumnName("first_name")
                .IsRequired();

            builder.Property(e => e.LastName)
                .HasMaxLength(64)
                .HasColumnName("last_name")
                .IsRequired();

            builder.Property(e => e.Email)
                .HasMaxLength(128)
                .HasColumnName("email")
                .IsRequired();

            builder.Property(e => e.Phone)
                .HasMaxLength(16)
                .HasColumnName("phone")
                .IsRequired();

            builder.Property(e => e.Country)
                .IsFixedLength()
                .HasMaxLength(3)
                .HasColumnName("country")
                .IsRequired();

            builder.Property(e => e.City)
                .HasMaxLength(128)
                .HasColumnName("city")
                .IsRequired();

            builder.Property(e => e.PostCode)
                .HasMaxLength(32)
                .HasColumnName("post_code");

            builder.Property(e => e.AddressLine)
                .HasMaxLength(512)
                .HasColumnName("address_line");

            builder.HasMany(e => e.Assignments)
                .WithOne(e => e.Employee)
                .HasForeignKey(e => e.EmployeeId);

            builder.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);
        }
    }
}
