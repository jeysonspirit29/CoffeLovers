using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidProductOrderException : DomainException
    {
        public InvalidProductOrderException()
            : base($"El producto agregado a la orden no es válido.")
        {
        }
    }
}
