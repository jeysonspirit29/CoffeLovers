using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidTotalOrderAmountException : DomainException
    {
        public InvalidTotalOrderAmountException(decimal amount)
            : base($"El monto total \"{amount}\" no es válido.")
        {
        }
    }
}
