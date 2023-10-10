using Domain.Domains.Products;
using Domain.Domains.RecipeDetails;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Domains.Recipes
{
    public sealed class Recipe : DomainBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public ICollection<RecipeDetail> RecipeDetails { get; set; } = new List<RecipeDetail>();
    }
}
