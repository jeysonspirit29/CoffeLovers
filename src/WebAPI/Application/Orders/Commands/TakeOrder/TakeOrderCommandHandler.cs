using Application.Common.Interfaces;
using Domain.Contants;
using Domain.DomainEvents;
using Domain.Domains.OrderDetails;
using Domain.Domains.Orders;
using Domain.Domains.Products;
using Domain.Enums;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Orders.Commands.TakeOrder
{

    public class TakeOrderCommandHandler : IRequestHandler<TakeOrderCommand, ErrorOr<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUser _user;

        public TakeOrderCommandHandler(IApplicationDbContext context, IUser user)
        {
            _context = context;
            _user = user;
        }

        public async Task<ErrorOr<bool>> Handle(TakeOrderCommand command, CancellationToken cancellationToken)
        {
            var order = _context.Orders.Find(command.OrderId);
            order.ChangeOrderStatus((OrderStatuses)command.OrderStatusId,_user.Id, _user.Role);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

    }
}
