using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Orders.CreateOrderItem
{
    public class CreateOrderItemCommand : IRequest<CreateOrderItemResult>
    {
        public Guid OrderId { get; }
        public IEnumerable<OrderItemCommand> OrderItems { get; set; }

        public CreateOrderItemCommand(Guid orderId, IEnumerable<OrderItemCommand> orderItems)
        {
            OrderId = orderId;
            OrderItems = orderItems;
        }

        public override string ToString()
        {
            return $"OrderId: {OrderId}, ItemsCount: [{OrderItems.Count()}]";
        }
    }
}