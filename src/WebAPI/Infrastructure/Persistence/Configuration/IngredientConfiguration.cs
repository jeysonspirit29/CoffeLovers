using Domain.Domains.Ingredients;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
    {
        public void Configure(EntityTypeBuilder<Ingredient> builder)
        {
            builder.ToTable("Ingredients");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).ValueGeneratedNever();

            builder.Property(c => c.Name).HasMaxLength(200);

            builder.Property(c => c.Unit).HasMaxLength(50);

            builder.Property(c => c.Stock).HasPrecision(18,6);

            builder.Property(c => c.Unit).HasMaxLength(50);
            
            builder.Property(c => c.LastModifiedBy)
                    .IsRequired(false)
                    .HasMaxLength(256);

            builder.Property(c => c.CreatedBy)
                    .HasMaxLength(256);

            builder.HasMany(e => e.RecipeDetails)
                   .WithOne(e => e.Ingredient)
                   .HasForeignKey(e => e.IngredientId)
                   .HasPrincipalKey(e => e.Id);

        }
    }
}