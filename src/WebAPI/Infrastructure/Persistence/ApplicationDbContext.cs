using Application.Common.Interfaces;
using Domain.Domains.Ingredients;
using Domain.Domains.OrderDetails;
using Domain.Domains.Orders;
using Domain.Domains.OrderStatuses;
using Domain.Domains.Products;
using Domain.Domains.RecipeDetails;
using Domain.Domains.Recipes;
using Domain.Domains.Tax;
using Domain.Domains.Users;
using Domain.Primitives;
using Infrastructure.Persistence.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly IPublisher _publisher;
    private readonly IUser _user;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPublisher publisher, IUser user) : base(options)
    {
        _publisher = publisher;
        _user = user;
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    public DbSet<OrderStatus> OrderStatuses => Set<OrderStatus>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<RecipeDetail> RecipeDetails => Set<RecipeDetail>();
    public DbSet<Tax> Taxes => Set<Tax>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        AuditEntityInterceptor();
        await DispatchDomainEvents(cancellationToken);
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void AuditEntityInterceptor()
    {
        foreach (var entry in ChangeTracker.Entries<DomainBase>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.DateCreated = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(_user.UserName))
                    entry.Entity.CreatedBy = _user.UserName;
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.DateLastModified = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(_user.UserName))
                    entry.Entity.LastModifiedBy = _user.UserName;
            }
        }
    }

    private async Task DispatchDomainEvents(CancellationToken cancellationToken = new CancellationToken())
    {
        var domainEvents = ChangeTracker.Entries<AggregateRoot>()
               .Select(e => e.Entity)
               .Where(e => e.DomainEvents.Any())
               .SelectMany(e => e.DomainEvents);

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }
    }


}
