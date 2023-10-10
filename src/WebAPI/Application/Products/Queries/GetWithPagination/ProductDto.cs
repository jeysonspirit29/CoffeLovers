using AutoMapper;
using Domain.Domains.OrderDetails;
using Domain.Domains.Orders;
using Domain.Domains.Products;
using System.Reflection;

namespace Application.Products.Queries.GetWithPagination
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhotoURL { get; set; }
        public decimal Price { get; set; }
        public decimal Stock { get; set; }
    }

    internal class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Product, ProductDto>();
        }
    }
}
