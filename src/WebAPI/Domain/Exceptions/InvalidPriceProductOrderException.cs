using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidPriceProductOrderException : DomainException
    {
        public InvalidPriceProductOrderException(string product, decimal price)
            : base($"El producto {product} no puede tener el precio: {price}.")
        {
        }
    }
}
