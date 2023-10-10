using ErrorOr;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Commands.Create
{
    public record CreateOrderCommand : IRequest<ErrorOr<int>>
    { 
        public int TaxId { get; init; }
        public ICollection<ItemOrderDetail> Items { get; init; } = new List<ItemOrderDetail>();
    }

    public record ItemOrderDetail
    {
        public int ProductId { get; init; }
        public decimal Quantity { get; init; }
    }
}
