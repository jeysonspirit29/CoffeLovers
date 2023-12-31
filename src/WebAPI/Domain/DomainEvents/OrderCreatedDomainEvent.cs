﻿using Domain.Domains.OrderDetails;
using Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainEvents
{
    public sealed record OrderCreatedDomainEvent(ICollection<OrderDetail> OrderDetails) :  DomainEvent
    {
    }
}
