using Domain.Domains.Products;
using Domain.Domains.Tax;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.Extensions;

public static class DbSetMocks
{
    public static Mock<DbSet<Tax>> GetMockDbSetTaxes()
    {
        var data = new List<Tax>
        {
            new Tax { Id = 1, TaxPercentage = TaxPercentage.Create(18m)}
        }
        .AsQueryable();
        var mockSet = new Mock<DbSet<Tax>>();
        mockSet.As<IQueryable<Tax>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<Tax>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<Tax>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<Tax>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        return mockSet;
    }

    public static Mock<DbSet<Product>> GetMockDbSetProducts()
    {
        var data = new List<Product>
        {
            new Product { Id = 1, Name = "Test Product 1", Price = 10, Stock = 15},
            new Product { Id = 2, Name = "Test Product 2", Price = -1, Stock = 15},
            new Product { Id = 3, Name = "Test Product 3", Price = 10, Stock = -1},
        }
        .AsQueryable();
        var mockSet = new Mock<DbSet<Product>>();
        mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(data.Provider);
        mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(data.Expression);
        mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(data.ElementType);
        mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
        return mockSet;
    }
}