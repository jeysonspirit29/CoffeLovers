using AutoMapper;
using Domain.Domains.OrderDetails;
using Domain.Domains.Orders;
using System.Reflection;

namespace Application.Orders.Queries.GetWithPagination
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string RequestingUser { get; set; }
        public string AttentionUser { get; set; }
        public string OrderStatus { get; set; }
        public DateTime? DateCompleted { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmountBeforeTax { get; set; }
        public decimal TotalOrderAmount { get; set; }
        public ICollection<OrderDetailDto> OrderDetails { get; set; } = new List<OrderDetailDto>();
    }

    public class OrderDetailDto
    {
        public string Product { get; set; }
        public string ProductPhotoURL { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal AmountSubtotal { get; set; }
    }

    internal class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Order, OrderDto>()
                 .ForMember(t => t.Id, m => m.MapFrom(s => s.Id))
                 .ForMember(t => t.RequestingUser, m => m.MapFrom(s => s.RequestingUser.FullName))
                 .ForMember(t => t.AttentionUser, m => m.MapFrom(s => s.AttentionUser.FullName))
                 .ForMember(t => t.OrderStatus, m => m.MapFrom(s => s.OrderStatus.Name))
                 .ForMember(t => t.DateCompleted, m => m.MapFrom(s => s.DateCompleted))
                 .ForMember(t => t.DateCreated, m => m.MapFrom(s => s.DateCreated))
                 .ForMember(t => t.TaxPercentage, m => m.MapFrom(s => s.TaxPercentage.Percentage))
                 .ForMember(t => t.TaxAmount, m => m.MapFrom(s => s.TaxAmount))
                 .ForMember(t => t.TotalAmountBeforeTax, m => m.MapFrom(s => s.TotalAmountBeforeTax))
                 .ForMember(t => t.TotalOrderAmount, m => m.MapFrom(s => s.TotalOrderAmount.TotalAmount))
                 .ForMember(t => t.OrderDetails, m => m.MapFrom(s => s.OrderDetails));


            CreateMap<OrderDetail, OrderDetailDto>()
                 .ForMember(t => t.ProductPhotoURL, m => m.MapFrom(s => s.Product.PhotoURL))
                 .ForMember(t => t.Product, m => m.MapFrom(s => s.Product.Name))
                 .ForMember(t => t.Quantity, m => m.MapFrom(s => s.Quantity))
                 .ForMember(t => t.Price, m => m.MapFrom(s => s.Price))
                 .ForMember(t => t.AmountSubtotal, m => m.MapFrom(s => s.AmountSubtotal));
        }
    }
}
