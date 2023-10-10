using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidQuantityProductOrderException : DomainException
    {
        public InvalidQuantityProductOrderException(string product, decimal quantity)
            : base($"El {product} no puede tener la cantidad: {quantity}.")
        {
        }
    }
}
