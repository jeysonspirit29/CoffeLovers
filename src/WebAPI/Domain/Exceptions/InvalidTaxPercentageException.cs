using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidTaxPercentageException : DomainException
    {
        public InvalidTaxPercentageException(decimal percentage)
            : base($"El porcentaje \"{percentage}\" no es válido.")
        {
        }
    }
}
