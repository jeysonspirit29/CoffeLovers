using Domain.Domains.Orders;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Domains.OrderStatuses
{
    public sealed class OrderStatus : DomainBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
