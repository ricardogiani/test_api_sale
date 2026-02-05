using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders.CreateOrder
{
    public class CreateOrderRequest
    {
        [Description("The unique identifier of the customer placing the order.")]
        public Guid CustomerId { get; set; }

        [Description("The date when the order was placed.")]
        public DateTime OrderDate { get; set; }

        [Description("The total amount of the order.")]
        public decimal TotalAmount { get; set; }

        [Description("The unique identifier of the branch where the order was placed.")]
        public Guid BranchId { get; set; }

        [Description("A collection of items included in the order.")]
        public IEnumerable<OrderItemRequest> OrderItems { get; set; }

        [Description("The unique identifier of the user who processed the order.")]
        public Guid UserId { get; set; }
    }
}