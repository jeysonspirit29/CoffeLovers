using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class TaxPercentage
    {
        private const decimal MaxPercentage = 100;
        private const decimal MinPercentage = 0;

        public static TaxPercentage Create(decimal percentage)
        {
            if (percentage < MinPercentage || percentage > MaxPercentage)
            {
                throw new InvalidTaxPercentageException(percentage);
            }
            return new TaxPercentage(percentage);
        }

        private TaxPercentage(decimal percentage)
        {
            Percentage = percentage;
        }

        public decimal Percentage { get; init; }
    }
}
