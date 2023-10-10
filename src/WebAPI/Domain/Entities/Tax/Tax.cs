using Domain.Domains.Orders;
using Domain.Primitives;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Domains.Tax
{
    public sealed class Tax : DomainBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TaxPercentage TaxPercentage { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
