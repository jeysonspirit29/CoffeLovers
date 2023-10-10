using Domain.Contants;
using Domain.Domains.Ingredients;
using Domain.Domains.OrderDetails;
using Domain.Domains.Orders;
using Domain.Domains.OrderStatuses;
using Domain.Domains.Products;
using Domain.Domains.RecipeDetails;
using Domain.Domains.Recipes;
using Domain.Domains.Tax;
using Domain.Domains.Users;
using Domain.Enums;
using Domain.ValueObjects;
using ErrorOr;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.Initialise();

        await initialiser.Seed();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger,
                                            ApplicationDbContext context,
                                            UserManager<ApplicationUser> userManager,
                                            RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task Initialise()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task Seed()
    {
        try
        {
            await TrySeed();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeed()
    {
        await AddRoles();
        var users = await AddUsers();
        var administrators = await AddAdministrators();
        var supervisors = await AddSupervisors(administrators);
        var employees = await AddEmployees(supervisors);

        var admin = GetAdmin(administrators);
        if (admin is null)
            return;

        await AddOrderStatuses(admin.UserName);
        await AddTaxes(admin.UserName);
        await AddIngredients(admin.UserName);
        await AddProducts(admin.UserName);
        await AddRecipes(admin.UserName);
        await AddOrders(users, employees, admin.UserName);

        await _context.SaveChangesAsync();
    }


    private async Task AddOrders(ICollection<ApplicationUser> users,
                                 ICollection<ApplicationUser> employees,
                                 string? admin)
    {
        var user1 = users.FirstOrDefault(x => x.UserName == "user1");
        var user2 = users.FirstOrDefault(x => x.UserName == "user2");
        var user3 = users.FirstOrDefault(x => x.UserName == "user3");

        var employee1 = employees.FirstOrDefault(x => x.UserName == "employee1");
        var employee2 = employees.FirstOrDefault(x => x.UserName == "employee2");
        var employee3 = employees.FirstOrDefault(x => x.UserName == "employee3");

        var orders = new List<Order>
        {
            new Order
            {
                TaxId = 1,
                OrderStatusId = (int)OrderStatuses.Pending,
                TaxPercentage = TaxPercentage.Create(18),
                TotalOrderAmount = TotalOrderAmount.Create(31),
                RequestingUserId = user1?.Id,
                CreatedBy = admin,
                OrderDetails = new[]
                {
                    new OrderDetail{ ProductId = 1, Quantity = 1, Price =  16, CreatedBy = admin },
                    new OrderDetail{ ProductId = 2, Quantity = 1, Price =  15, CreatedBy = admin }
                }
            },
            new Order
            {
                TaxId = 1,
                OrderStatusId = (int)OrderStatuses.InProgress,
                AttentionUserId = employee1.Id,
                TaxPercentage = TaxPercentage.Create(18),
                TotalOrderAmount = TotalOrderAmount.Create(30),
                RequestingUserId = user1?.Id,
                CreatedBy = admin,
                OrderDetails = new[]
                {
                    new OrderDetail{ ProductId = 5, Quantity = 2, Price =  15, CreatedBy = admin },
                }
            },
            new Order
            {
                TaxId = 1,
                AttentionUserId = employee2.Id,
                OrderStatusId = (int)OrderStatuses.Pending,
                TaxPercentage = TaxPercentage.Create(18),
                TotalOrderAmount = TotalOrderAmount.Create(26.5m),
                RequestingUserId = user2?.Id,
                CreatedBy = admin,
                OrderDetails = new[]
                {
                    new OrderDetail{ ProductId = 7, Quantity = 1, Price =  13, CreatedBy = admin },
                    new OrderDetail{ ProductId = 6, Quantity = 1, Price =  13.5m, CreatedBy = admin }
                }
            },
            new Order
            {
                TaxId = 1,
                AttentionUserId = employee2.Id,
                OrderStatusId = (int)OrderStatuses.Completed,
                TaxPercentage = TaxPercentage.Create(18),
                TotalOrderAmount = TotalOrderAmount.Create(15),
                RequestingUserId = user2?.Id,
                CreatedBy = admin,
                OrderDetails = new[]
                {
                    new OrderDetail{ ProductId = 2, Quantity = 1, Price =  15, CreatedBy = admin}
                }
            },
            new Order
            {
                TaxId = 1,
                AttentionUserId = employee3?.Id,
                OrderStatusId = (int)OrderStatuses.Delivered,
                TaxPercentage = TaxPercentage.Create(18),
                TotalOrderAmount = TotalOrderAmount.Create(11.5m),
                RequestingUserId = user3?.Id,
                CreatedBy = admin,
                OrderDetails = new[]
                {
                    new OrderDetail{ ProductId = 3, Quantity = 1, Price =  11.5m, CreatedBy = admin }
                }
            },
            new Order
            {
                TaxId = 1,
                OrderStatusId = (int)OrderStatuses.InProgress,
                AttentionUserId = employee3?.Id,
                TaxPercentage = TaxPercentage.Create(18),
                TotalOrderAmount = TotalOrderAmount.Create(15),
                RequestingUserId = user3?.Id,
                CreatedBy = admin,
                OrderDetails = new[]
                {
                    new OrderDetail{ ProductId = 2, Quantity = 1, Price =  15, CreatedBy = admin  },
                }
            }
        };

        var ordersCount = await _context.Orders.CountAsync();
        if (ordersCount > 0)
        {
            return;
        }

        foreach (var order in orders)
        {
            await _context.Orders.AddAsync(order);
        }
    }

    private async Task AddRecipes(string? admin)
    {
        var recipes = new List<Recipe>
        {
            new Recipe
            {
                ProductId = 1,
                Name = "Receta de Frappuccino",
                Note = "Para frappuccino de 600ml",
                CreatedBy = admin,
                RecipeDetails = new[]
                {
                    new RecipeDetail{ IngredientId = 1,Quantity = 0.01m, Comment = "Disolver en caliente", CreatedBy = admin },
                    new RecipeDetail{ IngredientId = 2,Quantity = 0.025m , CreatedBy = admin},
                    new RecipeDetail{ IngredientId = 4,Quantity = 0.1m , CreatedBy = admin},
                }
            },
            new Recipe
            {
                ProductId = 2,
                Name = "Receta de Espresso Caliente",
                Note = "Para espresso de 600ml",
                CreatedBy = admin,
                RecipeDetails = new[]
                {
                    new RecipeDetail{ IngredientId = 1,Quantity = 0.02m, Comment = "Disolver en caliente", CreatedBy = admin },
                    new RecipeDetail{ IngredientId = 2,Quantity = 0.02m, CreatedBy = admin },
                }
            },
            new Recipe
            {
                ProductId = 3,
                Name = "Receta de Espresso Frío",
                Note = "Para espresso de 600ml",
                CreatedBy = admin,
                RecipeDetails = new[]
                {
                    new RecipeDetail{ IngredientId = 1,Quantity = 0.02m, Comment = "Disolver en frío" , CreatedBy = admin},
                    new RecipeDetail{ IngredientId = 2,Quantity = 0.02m, CreatedBy = admin },
                    new RecipeDetail{ IngredientId = 3,Quantity = 8m, CreatedBy = admin },
                }
            },
            new Recipe
            {
                ProductId = 4,
                Name = "Receta de Shaken Espresso",
                Note = "Para shaken de 600ml",
                CreatedBy = admin,
                RecipeDetails = new[]
                {
                    new RecipeDetail{ IngredientId = 1,Quantity = 0.015m, Comment = "Disolver en caliente", CreatedBy = admin },
                    new RecipeDetail{ IngredientId = 2,Quantity = 0.025m, CreatedBy = admin },
                    new RecipeDetail{ IngredientId = 3,Quantity = 5m, CreatedBy = admin },
                    new RecipeDetail{ IngredientId = 7,Quantity = 0.1m, CreatedBy = admin },
                }
            },
            new Recipe
            {
                ProductId = 5,
                Name = "Receta de Jugo de Espinaca",
                Note = "Para jugo de 800ml",
                CreatedBy = admin,
                RecipeDetails = new[]
                {
                    new RecipeDetail{ IngredientId = 2,Quantity = 0.025m, CreatedBy = admin },
                    new RecipeDetail{ IngredientId = 3,Quantity = 8m, CreatedBy = admin },
                    new RecipeDetail{ IngredientId = 4,Quantity = 0.2m, CreatedBy = admin },
                    new RecipeDetail{ IngredientId = 5,Quantity = 0.3m, CreatedBy = admin },
                }
            },
            new Recipe
            {
                ProductId = 6,
                Name = "Receta de Strawberry Acaí Refresher",
                Note = "Para frappuccino de 600ml",
                CreatedBy = admin,
                RecipeDetails = new[]
                {
                    new RecipeDetail{ IngredientId = 2,Quantity = 0.025m , CreatedBy = admin},
                    new RecipeDetail{ IngredientId = 3,Quantity = 8m , CreatedBy = admin},
                    new RecipeDetail{ IngredientId = 4,Quantity = 0.2m , CreatedBy = admin},
                    new RecipeDetail{ IngredientId = 6,Quantity = 0.3m , CreatedBy = admin},
                }
            },
            new Recipe
            {
                ProductId = 7,
                Name = "Receta de Chocolate",
                Note = "Para frappuccino de 600ml",
                CreatedBy = admin,
                RecipeDetails = new[]
                {
                    new RecipeDetail{ IngredientId = 1,Quantity = 0.01m, Comment = "Disolver en frío" , CreatedBy = admin},
                    new RecipeDetail{ IngredientId = 2,Quantity = 0.025m , CreatedBy = admin},
                    new RecipeDetail{ IngredientId = 3,Quantity = 5m, CreatedBy = admin },
                    new RecipeDetail{ IngredientId = 4,Quantity = 0.1m , CreatedBy = admin},
                    new RecipeDetail{ IngredientId = 8,Quantity = 0.1m , CreatedBy = admin},
                }
            }
        };

        foreach (var recipe in recipes)
        {
            bool exists = await _context.Recipes.AnyAsync(x => x.ProductId == recipe.ProductId);
            if (!exists)
            {
                await _context.Recipes.AddAsync(recipe);
            }
        }
    }

    private async Task AddIngredients(string? admin)
    {
        var ingredients = new List<Ingredient>
        {
            new Ingredient(){ Id = 1, Name = "Café", Stock = 5, Unit = "kg", CreatedBy = admin },
            new Ingredient(){ Id = 2, Name = "Azúcar", Stock = 10, Unit = "kg", CreatedBy = admin },
            new Ingredient(){ Id = 3, Name = "Hielo", Stock = 600, Unit = "cubos", CreatedBy = admin },
            new Ingredient(){ Id = 4, Name = "Leche", Stock = 20, Unit = "lt", CreatedBy = admin },
            new Ingredient(){ Id = 5, Name = "Espinaca", Stock = 15, Unit = "kg", CreatedBy = admin },
            new Ingredient(){ Id = 6, Name = "Fresa", Stock = 8, Unit = "kg", CreatedBy = admin },
            new Ingredient(){ Id = 7, Name = "Lúcuma", Stock = 0, Unit = "kg", CreatedBy = admin },
            new Ingredient(){ Id = 8, Name = "Chocolate", Stock = 25, Unit = "kg", CreatedBy = admin }
        };

        foreach (var ingredient in ingredients)
        {
            bool exists = await _context.Ingredients.AnyAsync(x => x.Id == ingredient.Id);
            if (!exists)
            {
                await _context.Ingredients.AddAsync(ingredient);
            }
        }
    }

    private async Task AddProducts(string? admin)
    {
        var products = new List<Product>
        {
            new Product(){ Id = 1, Name = "Frappuccino", Price = 16, PhotoURL = "https://www.starbucks.pe/Multimedia/subsecciones/ULTIMATE_CARAMEL_FRAPP_V3.png", CreatedBy = admin  },
            new Product(){ Id = 2, Name = "Espresso Caliente", Price = 15, PhotoURL = "https://www.starbucks.pe/Multimedia/subsecciones/LATTE_MACHIATTO_V3.png", CreatedBy = admin  },
            new Product(){ Id = 3, Name = "Espresso Frío", Price = 11.5m, PhotoURL = "https://www.starbucks.pe/Multimedia/subsecciones/CARAMEL_MACCHIATO_HELADO_V4.png", CreatedBy = admin },
            new Product(){ Id = 4, Name = "Shaken Espresso", Price = 16.5m, PhotoURL = "https://www.starbucks.pe/Multimedia/subsecciones/ICED_AVELLANA_OATMILK_SHAKEN_ESPRESSO_V1.png", CreatedBy = admin  },
            new Product(){ Id = 5, Name = "Jugo de Espinaca", Price = 9.5m, PhotoURL = "https://www.starbucks.pe/Multimedia/productos/JUGO_DE_ESPINACA_Y_MANZANA_V2.png" , CreatedBy = admin },
            new Product(){ Id = 6, Name = "Strawberry Acaí Refresher", Price = 13.5m, PhotoURL = "https://www.starbucks.pe/Multimedia/productos/STRAWBERRY_ACAI_REFRESHER_V2.png", CreatedBy = admin  },
            new Product(){ Id = 7, Name = "Chocolate Helado", Price = 13m, PhotoURL = "https://www.starbucks.pe/Multimedia/productos/CHOCOLATE_HELADO_V2.png" , CreatedBy = admin }
        };

        foreach (var product in products)
        {
            bool exists = await _context.Products.AnyAsync(x => x.Id == product.Id);
            if (!exists)
            {
                if (product.Id != 4) 
                { 
                    product.ChangeStock(25);
                }
                await _context.Products.AddAsync(product);
            }
        }
    }

    private async Task AddOrderStatuses(string admin)
    {
        var ordeStatuses = new List<OrderStatus>
        {
            new OrderStatus(){ Id = (int)OrderStatuses.Pending, Name = "Pending", CreatedBy = admin },
            new OrderStatus(){ Id = (int)OrderStatuses.InProgress, Name = "InProgress", CreatedBy = admin},
            new OrderStatus(){ Id = (int)OrderStatuses.Delivered, Name = "Delivered",CreatedBy = admin },
            new OrderStatus(){ Id = (int)OrderStatuses.Completed, Name = "Completed", CreatedBy = admin }
        };

        foreach (var ordeStatus in ordeStatuses)
        {
            bool exists = await _context.OrderStatuses.AnyAsync(x => x.Id == ordeStatus.Id);
            if (!exists)
            {
                await _context.OrderStatuses.AddAsync(ordeStatus);
            }
        }
    }

    private async Task AddTaxes(string admin)
    {
        var taxes = new List<Tax>
        {
            new Tax(){ Id = 1, Name = "IMPUESTO ESTÁNDAR 18%", TaxPercentage = Domain.ValueObjects.TaxPercentage.Create(18), CreatedBy = admin },
            new Tax(){ Id = 2, Name = "IMPUESTO A 15%", TaxPercentage = Domain.ValueObjects.TaxPercentage.Create(15), CreatedBy = admin },
            new Tax(){ Id = 3, Name = "IMPUESTO B 20%", TaxPercentage = Domain.ValueObjects.TaxPercentage.Create(20), CreatedBy = admin }
        };

        foreach (var tax in taxes)
        {
            bool exists = await _context.Taxes.AnyAsync(x => x.Id == tax.Id);
            if (!exists)
            {
                await _context.Taxes.AddAsync(tax);
            }
        }
    }

    private async Task<ICollection<ApplicationUser>> AddUsers()
    {
        var users = new List<ApplicationUser>
        {
            new ApplicationUser { UserName = "user1", Email = "user1@caffelovers.com", Name = "Jeyson", LastName = "Soto" },
            new ApplicationUser { UserName = "user2", Email = "user2@caffelovers.com", Name = "Manuel", LastName = "Rosales" },
            new ApplicationUser { UserName = "user3", Email = "user3@caffelovers.com", Name = "Carlos", LastName = "Tello"},
            new ApplicationUser { UserName = "user4", Email = "user4@caffelovers.com", Name = "Maria", LastName = "Lopez" },
            new ApplicationUser { UserName = "user5", Email = "user5@caffelovers.com", Name = "Cinthya", LastName = "Minaya" }
        };
        return await AddUsersWithRol(users, Roles.User);
    }

    private async Task<ICollection<ApplicationUser>> AddEmployees(ICollection<ApplicationUser> supervisors)
    {
        var supervisor1 = supervisors.FirstOrDefault(x => x.UserName == "supervisor1");
        var supervisor2 = supervisors.FirstOrDefault(x => x.UserName == "supervisor2");
        var employees = new List<ApplicationUser>
        {
            new ApplicationUser { UserName = "employee1", Email = "employee1@caffelovers.com", Name = "Siler", LastName = "Jara", ChiefUserId = supervisor1?.Id  },
            new ApplicationUser { UserName = "employee2", Email = "employee2@caffelovers.com", Name = "Elvis", LastName = "Jara", ChiefUserId = supervisor1?.Id },
            new ApplicationUser { UserName = "employee3", Email = "employee3@caffelovers.com", Name = "Luis", LastName = "Jara", ChiefUserId = supervisor2?.Id }
        };
        return await AddUsersWithRol(employees, Roles.Employee);
    }

    private async Task<ICollection<ApplicationUser>> AddSupervisors(ICollection<ApplicationUser> administrators)
    {
        var admin = GetAdmin(administrators);
        var supervisors = new List<ApplicationUser>
        {
            new ApplicationUser { UserName = "supervisor1", Email = "supervisor1@caffelovers.com", Name = "Luisa", LastName = "Flores", ChiefUserId = admin?.Id },
            new ApplicationUser { UserName = "supervisor2", Email = "supervisor2@caffelovers.com", Name = "Lucas", LastName = "Sifuentes", ChiefUserId = admin?.Id }
        };
        return await AddUsersWithRol(supervisors, Roles.Supervisor);
    }

    private ApplicationUser GetAdmin(ICollection<ApplicationUser> administrators)
    {
        return administrators.FirstOrDefault(x => x.UserName == "admin1");
    }

    private async Task<ICollection<ApplicationUser>> AddAdministrators()
    {
        var administrators = new List<ApplicationUser>
        {
            new ApplicationUser { UserName = "admin1", Email = "admin1@caffelovers.com", Name = "Bene", LastName = "Jara" }
        };
        return await AddUsersWithRol(administrators, Roles.Administrator);
    }

    private async Task<ICollection<ApplicationUser>> AddUsersWithRol(List<ApplicationUser> users, string rol)
    {
        var addedUsers = new List<ApplicationUser>();
        foreach (var user in users)
        {
            bool exists = _userManager.Users.Any(u => u.UserName == user.UserName);
            if (!exists)
            {
                var result = await _userManager.CreateAsync(user, "abc123");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, rol.ToUpper());
                    addedUsers.Add(user);
                }
            }
        }
        return addedUsers;
    }

    private async Task AddRoles()
    {
        var roles = new List<IdentityRole>
        {
            new IdentityRole(Roles.User),
            new IdentityRole(Roles.Employee),
            new IdentityRole(Roles.Supervisor),
            new IdentityRole(Roles.Administrator)
        };

        foreach (var rol in roles)
        {
            bool exists = _roleManager.Roles.Any(r => r.Name == rol.Name);
            if (!exists)
            {
                await _roleManager.CreateAsync(rol);
            }
        }
    }

}
