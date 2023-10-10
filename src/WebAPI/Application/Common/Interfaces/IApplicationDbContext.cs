using Domain.Domains.Ingredients;
using Domain.Domains.OrderDetails;
using Domain.Domains.Orders;
using Domain.Domains.OrderStatuses;
using Domain.Domains.Products;
using Domain.Domains.RecipeDetails;
using Domain.Domains.Recipes;
using Domain.Domains.Tax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Order> Orders { get; }
        DbSet<OrderDetail> OrderDetails { get; }
        DbSet<OrderStatus> OrderStatuses { get; }
        DbSet<Ingredient> Ingredients { get; }
        DbSet<Product> Products { get; }
        DbSet<Recipe> Recipes { get; }
        DbSet<RecipeDetail> RecipeDetails { get; }
        DbSet<Tax> Taxes { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
