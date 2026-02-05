using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Orders.DeleteOrderItem
{
    public class DeleteOrderItemCommand : IRequest<DeleteOrderItemResult>
    {
        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        /// <summary>
        /// /// Initializes a new instance of DeleteOrderItemCommand.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order.</param>
        /// <param name="ProductId">The unique identifier of the order item.</param>
        public DeleteOrderItemCommand(Guid orderId, Guid productId)
        {
            OrderId = orderId;
            ProductId = productId;
        }

        public override string ToString()
        {
            return $"OrderId: {OrderId}, ProductId: {ProductId}";
        }
        

    }
}