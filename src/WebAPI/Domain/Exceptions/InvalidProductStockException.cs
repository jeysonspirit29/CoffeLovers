using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidProductStockException : DomainException
    {
        public InvalidProductStockException(string product, decimal stock)
            : base($"El producto {product} no puede tener stock: {stock}.")
        {
        }
    }
}
