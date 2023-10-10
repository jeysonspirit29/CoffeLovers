using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Commands.TakeOrder
{
    public record TakeOrderCommand : IRequest<ErrorOr<bool>>
    { 
        public int OrderId { get; init; }
        public int OrderStatusId { get; init; }
    }
}
