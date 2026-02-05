using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders
{
    public class OrderItemRequest
    {
        public Guid ProductId { get; set; }

        public long Quantity { get; set; }

    }
}