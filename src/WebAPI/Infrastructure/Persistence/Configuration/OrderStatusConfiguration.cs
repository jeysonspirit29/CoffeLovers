using Domain.Domains.OrderStatuses;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class OrderStatusConfiguration : IEntityTypeConfiguration<OrderStatus>
    {
        public void Configure(EntityTypeBuilder<OrderStatus> builder)
        {
            builder.ToTable("OrderStatuses");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).ValueGeneratedNever();

            builder.Property(c => c.Name).HasMaxLength(200);

            builder.Property(c => c.Description).HasMaxLength(750).IsRequired(false);

            builder.Property(c => c.LastModifiedBy)
                    .IsRequired(false)
                    .HasMaxLength(256);

            builder.Property(c => c.CreatedBy)
                    .HasMaxLength(256);

            builder.HasMany(e => e.Orders)
                   .WithOne(e => e.OrderStatus)
                   .HasForeignKey(e => e.OrderStatusId)
                   .HasPrincipalKey(e => e.Id);

        }
    }
}