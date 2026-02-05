using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Common
{
    public static class DomainSettings
    {
        public static (decimal Discount, int TotalItems) Discount20_Percent = (0.2M, 9);

        public static (decimal Discount, int TotalItems) Discount10_Percent = (0.1M, 4);

        public static decimal NothingDiscount = 0.0M;

        public static decimal MaxSameItemsToOrder = 20;
        
    }
}