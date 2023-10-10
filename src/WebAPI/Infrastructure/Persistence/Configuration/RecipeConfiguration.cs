using Domain.Domains.Recipes;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.ToTable("Recipes");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.Name).HasMaxLength(200);

            builder.Property(c => c.Note).HasMaxLength(750).IsRequired(false);

            builder.Property(c => c.LastModifiedBy)
                    .IsRequired(false)
                    .HasMaxLength(256);

            builder.Property(c => c.CreatedBy)
                    .HasMaxLength(256);

            builder.HasMany(e => e.RecipeDetails)
                   .WithOne(e => e.Recipe)
                   .HasForeignKey(e => e.RecipeId)
                   .HasPrincipalKey(e => e.Id);

        }
    }
}