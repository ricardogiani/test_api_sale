using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Enums
{
    public enum OrderStatus
    {
        Unknown = 0,
        Pending = 1,
        Confirmed = 2,
        Completed = 3,
        Canceled = 4

    }
}