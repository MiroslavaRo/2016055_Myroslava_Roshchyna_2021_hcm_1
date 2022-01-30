using Hcm.Database.Common;
using Hcm.Database.Constants;
using Hcm.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hcm.Database.Mappings
{
    public class SalaryMap : IEntityTypeConfiguration<Sallary>
    {
        public void Configure(EntityTypeBuilder<Sallary> builder)
        {
            builder.ToTable(Tables.Salaries, "dbo");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasValueGenerator<IdValueGenerator>()
                .HasColumnName("id")
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(e => e.Amount)
                .HasColumnType("numeric(18,2)")
                .HasColumnName("amount")
                .IsRequired();

            builder.Property(e => e.Currency)
                .IsFixedLength()
                .HasMaxLength(3)
                .HasColumnName("currency")
                .IsRequired();

            builder.Property(e => e.AssignmentId)
                .HasColumnName("assignment_id")
                .IsRequired();

            builder.HasOne(e => e.Assignment)
                .WithMany(e => e.Sallaries)
                .HasForeignKey(e => e.AssignmentId)
                .IsRequired();
        }
    }
}
