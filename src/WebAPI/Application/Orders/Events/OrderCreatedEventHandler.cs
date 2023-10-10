using Application.Common.Interfaces;
using Domain.DomainEvents;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Events
{
    public sealed class OrderCreatedEventHandler : INotificationHandler<OrderCreatedDomainEvent>
    {
        private readonly IApplicationDbContext _context;

        public OrderCreatedEventHandler(IApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var productsId = notification.OrderDetails
                                            .Select(x => x.ProductId)
                                            .ToList();

            var productsToUpdate = _context.Products
                                            .Where(x => productsId.Contains(x.Id))
                                            .ToList();

            foreach (var orderDetail in notification.OrderDetails)
            {
                var productToUpdate = productsToUpdate.First(x => x.Id == orderDetail.ProductId);
                productToUpdate.ChangeStock(-orderDetail.Quantity);
            }
        }
    }
}
