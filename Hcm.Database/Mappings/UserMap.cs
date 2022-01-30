using Hcm.Database.Common;
using Hcm.Database.Constants;
using Hcm.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hcm.Database.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(Tables.Users, "dbo");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasValueGenerator<IdValueGenerator>()
                .HasColumnName("id")
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(e => e.Username)
                .HasMaxLength(128)
                .HasColumnName("username")
                .IsRequired();

            builder.Property(e => e.Password)
               .HasMaxLength(1024)
               .HasColumnName("password")
               .IsRequired();

            builder.Property(e => e.Email)
                .HasMaxLength(128)
                .HasColumnName("email")
                .IsRequired();

            builder.Property(e => e.Phone)
                .HasMaxLength(16)
                .HasColumnName("phone");

            builder.Property(e => e.Role)
                .HasMaxLength(16)
                .HasColumnName("role")
                .IsRequired();
        }
    }
}
