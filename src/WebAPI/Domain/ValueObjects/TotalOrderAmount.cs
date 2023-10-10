using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class TotalOrderAmount
    {
        private const decimal MaxAmount = (decimal)999999999999.999999f;
        private const decimal MinAmount = 0;

        public static TotalOrderAmount Create(decimal amount)
        {
            if (amount < MinAmount || amount > MaxAmount)
            {
                throw new InvalidTaxPercentageException(amount);
            }
            return new TotalOrderAmount(amount);
        }

        private TotalOrderAmount(decimal amount)
        {
            TotalAmount = amount;
        }

        public decimal TotalAmount { get; init; }
    }
}
