using Domain.Domains.Products;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).ValueGeneratedNever();

            builder.Property(c => c.Name).HasMaxLength(200);

            builder.Property(c => c.Stock).HasPrecision(18, 6);

            builder.Property(c => c.PhotoURL).HasMaxLength(2083).IsRequired(false);

            builder.Property(c => c.Price).HasPrecision(12,2);

            builder.Property(c => c.LastModifiedBy)
                    .IsRequired(false)
                    .HasMaxLength(256);

            builder.Property(c => c.CreatedBy)
                    .HasMaxLength(256);

            builder.HasMany(e => e.Recipes)
                   .WithOne(e => e.Product)
                   .HasForeignKey(e => e.ProductId)
                   .HasPrincipalKey(e => e.Id);

            builder.HasMany(e => e.OrderDetails)
                   .WithOne(e => e.Product)
                   .HasForeignKey(e => e.ProductId)
                   .HasPrincipalKey(e => e.Id);
        }
    }
}