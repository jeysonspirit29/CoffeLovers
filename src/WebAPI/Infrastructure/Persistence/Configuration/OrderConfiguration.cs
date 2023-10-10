using Domain.Domains.Orders;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.RequestingUserId).HasMaxLength(450);

            builder.Property(c => c.AttentionUserId).HasMaxLength(450).IsRequired(false);

            builder.Property(c => c.TaxPercentage)
                    .HasConversion( taxPercentage => taxPercentage.Percentage,
                                    value => TaxPercentage.Create(value))
                    .HasPrecision(5,2);

            builder.Property(c => c.TotalOrderAmount)
                    .HasConversion(totalOrderAmount => totalOrderAmount.TotalAmount,
                                    value => TotalOrderAmount.Create(value))
                    .HasPrecision(18, 6);

            builder.Property(c => c.LastModifiedBy)
                    .IsRequired(false)
                    .HasMaxLength(256);

            builder.Property(c => c.CreatedBy)
                    .HasMaxLength(256);

            builder.Ignore(c => c.TaxPercentageDecimal);
            builder.Ignore(c => c.TaxAmount);
            builder.Ignore(c => c.TotalAmountBeforeTax);

            builder.HasMany(e => e.OrderDetails)
                   .WithOne(e => e.Order)
                   .HasForeignKey(e => e.OrderId)
                   .HasPrincipalKey(e => e.Id);

        }
    }
}