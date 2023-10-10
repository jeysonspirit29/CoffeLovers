using Domain.Domains.OrderDetails;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OrderDetails");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.Quantity).HasPrecision(12,2);
            
            builder.Property(c => c.Price).HasPrecision(18,6);

            builder.Ignore(c => c.AmountSubtotal);

            builder.Property(c => c.LastModifiedBy)
                    .IsRequired(false)
                    .HasMaxLength(256);

            builder.Property(c => c.CreatedBy)
                    .HasMaxLength(256);

        }
    }
}