using Domain.Domains.Users;
using Domain.Domains.Orders;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {

            builder.Property(c => c.Name).HasMaxLength(128);

            builder.Property(c => c.LastName).HasMaxLength(128).IsRequired(false);

            builder.Property(c => c.ChiefUserId).HasMaxLength(450).IsRequired(false);

            builder.Ignore(c => c.FullName);

            builder.HasMany(e => e.RequestedOrders)
                       .WithOne(e => e.RequestingUser)
                       .HasForeignKey(e => e.RequestingUserId)
                       .HasPrincipalKey(e => e.Id);

            builder.HasMany(e => e.FilledOrders)
                   .WithOne(e => e.AttentionUser)
                   .HasForeignKey(e => e.AttentionUserId)
                   .HasPrincipalKey(e => e.Id);

        }
    }
}