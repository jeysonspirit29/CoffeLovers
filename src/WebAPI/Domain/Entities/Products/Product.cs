using Domain.Domains.OrderDetails;
using Domain.Domains.Recipes;
using Domain.Exceptions;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Domains.Products
{
    public sealed class Product : DomainBase
    {
        public decimal _stock;
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhotoURL { get; set; }
        public decimal Stock { get => _stock; init => _stock = value; }
        public decimal Price { get; set; }
        public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public void ChangeStock(decimal quantity)
        {
            decimal newStock = Stock + quantity;
            if (newStock < 0)
            {
                throw new InvalidProductStockException(Name, newStock);
            }
            _stock = newStock;
        }
    }
}
