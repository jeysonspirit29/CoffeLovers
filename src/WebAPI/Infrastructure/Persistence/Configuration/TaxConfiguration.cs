using Domain.Domains.Tax;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class TaxConfiguration : IEntityTypeConfiguration<Tax>
    {
        public void Configure(EntityTypeBuilder<Tax> builder)
        {
            builder.ToTable("Taxes");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).ValueGeneratedNever();

            builder.Property(c => c.Name).HasMaxLength(200);

            builder.Property(c => c.TaxPercentage)
                      .HasConversion(taxPercentage => taxPercentage.Percentage,
                                      value => TaxPercentage.Create(value))
                      .HasPrecision(5, 2);

            builder.Property(c => c.LastModifiedBy)
                    .IsRequired(false)
                    .HasMaxLength(256);

            builder.Property(c => c.CreatedBy)
                    .HasMaxLength(256);

            builder.HasMany(e => e.Orders)
                   .WithOne(e => e.Tax)
                   .HasForeignKey(e => e.TaxId)
                   .HasPrincipalKey(e => e.Id);

        }
    }
}