using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidOrderStatusChangeException : DomainException
    {
        public InvalidOrderStatusChangeException(OrderStatuses from, OrderStatuses to, string role)
            : base($"Pasar del estado {from} al estado {to} no es válido para el rol {role}.")
        {
        }
    }
}
