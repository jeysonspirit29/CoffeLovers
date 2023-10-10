using Application.Common.Interfaces;
using Application.Orders.Commands.Create;
using Application.UnitTests.Extensions;
using Castle.Core.Resource;
using Domain.Domains.Tax;
using Domain.ValueObjects;
using FluentAssertions;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Net.Sockets;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;
using Azure.Core;
using System.Runtime.InteropServices;
using System.Threading;
using Domain.Exceptions;

namespace Application.UnitTests.CreateOrder;

public class CreateOrderCommandHandlerUnitTests
{
    private readonly Mock<IApplicationDbContext> _context;
    private readonly Mock<IUser> _user;
    private readonly CreateOrderCommandHandler _handler;

    public CreateOrderCommandHandlerUnitTests()
    {
        _user = new Mock<IUser>();
        _context = new Mock<IApplicationDbContext>();
        var mockSetTax = DbSetMocks.GetMockDbSetTaxes();
        _context.Setup(c => c.Taxes).Returns(mockSetTax.Object);
        var mockSetProduct = DbSetMocks.GetMockDbSetProducts();
        _context.Setup(c => c.Products).Returns(mockSetProduct.Object);
        _handler = new CreateOrderCommandHandler(_context.Object, _user.Object);
    }


    [Fact]
    public async Task HandleCreateOrder_WhenProductIsInvalid_ShoouldReturnValidationError()
    {
        //Arrange - Parámetros de entrada
        var command = new CreateOrderCommand()
        {
            TaxId = 1,
            Items = new List<ItemOrderDetail>() {
                new ItemOrderDetail()
                {
                    ProductId = -1,
                    Quantity = 1
                }
            }
        };

        //Act - Ejecución
        Func<Task> action = () => _handler.Handle(command, default);

        //Assert - Evalua datos de retorno 
        await Assert.ThrowsAsync<InvalidProductOrderException>(action);
    }

    [Fact]
    public async Task HandleCreateOrder_WhenQuantityIsInvalid_ShoouldReturnValidationError()
    {
        //Arrange - Parámetros de entrada
        var command = new CreateOrderCommand()
        {
            TaxId = 1,
            Items = new List<ItemOrderDetail>() {
                new ItemOrderDetail()
                {
                    ProductId = 1,
                    Quantity = -1
                }
            }
        };

        //Act - Ejecución
        Func<Task> action = () => _handler.Handle(command, default);

        //Assert - Evalua datos de retorno 
        await Assert.ThrowsAsync<InvalidQuantityProductOrderException>(action);
    }

    [Fact]
    public async Task HandleCreateOrder_WhenPriceIsInvalid_ShoouldReturnValidationError()
    {
        //Arrange - Parámetros de entrada
        var command = new CreateOrderCommand()
        {
            TaxId = 1,
            Items = new List<ItemOrderDetail>() {
                new ItemOrderDetail()
                {
                    ProductId = 2,
                    Quantity = 1
                }
            }
        };

        //Act - Ejecución
        Func<Task> action = () => _handler.Handle(command, default);

        //Assert - Evalua datos de retorno 
        await Assert.ThrowsAsync<InvalidPriceProductOrderException>(action);
    }
}
