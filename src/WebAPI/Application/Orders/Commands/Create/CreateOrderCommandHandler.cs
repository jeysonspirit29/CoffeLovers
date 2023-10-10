using Application.Common.Interfaces;
using Domain.DomainEvents;
using Domain.Domains.OrderDetails;
using Domain.Domains.Orders;
using Domain.Domains.Products;
using Domain.Enums;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Orders.Commands.Create
{

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ErrorOr<int>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUser _user;

        public CreateOrderCommandHandler(IApplicationDbContext context, IUser user)
        {
            _context = context;
            _user = user;
        }

        public async Task<ErrorOr<int>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                OrderStatusId = (int)OrderStatuses.Pending,
                RequestingUserId = _user.Id,
                TaxId = command.TaxId
            };

            EstablishTax(command, order);
            SetOrderDetails(command, order);
            order.CalculateTotalOrderAmount();

            _context.Orders.Add(order);

            order.RaiseDomainEvent(new OrderCreatedDomainEvent(order.OrderDetails));

            await _context.SaveChangesAsync(cancellationToken);
            return order.Id;
        }

        private void EstablishTax(CreateOrderCommand command, Order order)
        {
            var tax = _context.Taxes.FirstOrDefault(x => x.Id == command.TaxId);
            order.SetTax(tax);
        }

        private void SetOrderDetails(CreateOrderCommand command, Order order)
        {
            var productIds = command.Items.Select(x => x.ProductId);
            var products = _context.Products.Where(x => productIds.Contains(x.Id)).ToList();
            foreach (var item in command.Items)
            {
                var product = products.FirstOrDefault(x => x.Id == item.ProductId);
                order.AddProduct(product, item.Quantity);
            }
        }
    }
}
