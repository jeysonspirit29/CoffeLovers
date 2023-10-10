using Domain.Domains.Orders;
using Domain.Primitives;
using Microsoft.AspNetCore.Identity;

namespace Domain.Domains.Users
{
    public sealed class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{Name} {LastName}";
        public string ChiefUserId { get; set; }
        public ICollection<Order> RequestedOrders { get; set; } = new List<Order>();
        public ICollection<Order> FilledOrders { get; set; } = new List<Order>();
    }
}
