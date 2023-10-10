using Domain.Domains.OrderDetails;
using Domain.Domains.OrderStatuses;
using Domain.Domains.Users;
using Domain.Domains.Tax;
using Domain.Primitives;
using Domain.ValueObjects;
using Domain.Enums;
using Domain.Contants;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Domain.Domains.Products;

namespace Domain.Domains.Orders
{
    public sealed class Order : DomainBase
    {

        public int Id { get; set; }
        public string RequestingUserId { get; set; }
        public ApplicationUser RequestingUser { get; set; }
        public string AttentionUserId { get; set; }
        public ApplicationUser AttentionUser { get; set; }
        public int TaxId { get; set; }
        public Domain.Domains.Tax.Tax Tax { get; set; }
        public int OrderStatusId { get => _orderStatusId; init => _orderStatusId = value; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime? DateCompleted { get; set; }
        public TaxPercentage TaxPercentage { get; set; }
        public decimal TaxPercentageDecimal => TaxPercentage.Percentage > 0 ? 100 / TaxPercentage.Percentage : 0;
        public decimal TaxAmount => TotalAmountBeforeTax * TaxPercentageDecimal;
        public decimal TotalAmountBeforeTax => TotalOrderAmount.TotalAmount / (1 + TaxPercentageDecimal);
        public TotalOrderAmount TotalOrderAmount { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        private int _orderStatusId;


        public void SetTax(Domains.Tax.Tax tax)
        {
            TaxPercentage = TaxPercentage.Create(tax.TaxPercentage.Percentage);
        }

        public void CalculateTotalOrderAmount()
        {
            decimal totalAmount = OrderDetails.Sum(x => x.AmountSubtotal);
            TotalOrderAmount = TotalOrderAmount.Create(totalAmount);
        }

        public void AddProduct(Product product, decimal quantity)
        {
            if (product is null || product.Id <= 0)
            {
                throw new InvalidProductOrderException();
            }
            if (product.Price <= 0)
            {
                throw new InvalidPriceProductOrderException(product.Name, product.Price);
            }
            if (quantity <= 0)
            {
                throw new InvalidQuantityProductOrderException(product.Name, quantity);
            }

            OrderDetails.Add(new OrderDetail
            {
                ProductId = product.Id,
                Price = product.Price,
                Quantity = quantity
            });
        }

        public void ChangeOrderStatus(Enums.OrderStatuses to, string userId, string role)
        {
            if (((Enums.OrderStatuses)OrderStatusId == Enums.OrderStatuses.Pending &&
                (to != Enums.OrderStatuses.InProgress || role != Roles.Employee)) ||
               ((Enums.OrderStatuses)OrderStatusId == Enums.OrderStatuses.InProgress &&
               (to != Enums.OrderStatuses.Delivered || role != Roles.Employee)) ||
               ((Enums.OrderStatuses)OrderStatusId == Enums.OrderStatuses.Delivered &&
               (to != Enums.OrderStatuses.Completed || (role != Roles.Supervisor && role != Roles.Administrator))) ||
               ((Enums.OrderStatuses)OrderStatusId == Enums.OrderStatuses.Completed))
            {
                throw new InvalidOrderStatusChangeException((Enums.OrderStatuses)OrderStatusId, to, role);
            }
            if ((Enums.OrderStatuses)OrderStatusId == Enums.OrderStatuses.Pending)
            {
                AttentionUserId = userId;
            }
            if (to == Enums.OrderStatuses.Completed)
            {
                DateCompleted = DateTime.Now;
            }
            _orderStatusId = (int)to;
        }
    }
}
