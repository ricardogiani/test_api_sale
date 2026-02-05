using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders.CreateOrderItem
{
    public class CreateOrderItemRequest
    {
        public Guid OrderId { get; set; }
        
        public IEnumerable<OrderItemRequest> OrderItems { get; set; }
    }
}