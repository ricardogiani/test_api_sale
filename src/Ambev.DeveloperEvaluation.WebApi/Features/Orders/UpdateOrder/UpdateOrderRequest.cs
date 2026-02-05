using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders.UpdateOrder
{
    public class UpdateOrderRequest
    {
        public string Status { get; set; }
        public DateTime? OrderDate { get; set; }
        
    }
}