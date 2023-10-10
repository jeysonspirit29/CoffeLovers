using Domain.Domains.RecipeDetails;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Domains.Ingredients
{
    public sealed class Ingredient : DomainBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Stock { get; set; }
        public string Unit { get; set; }
        public ICollection<RecipeDetail> RecipeDetails { get; set; } = new List<RecipeDetail>();
    }
}
