using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Orders.CreateOrder;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Orders
{
    public class OrderSpecification
    {
        public async Task ValidateOrderItems(IEnumerable<OrderItemCommand> orderItemsCmd, CancellationToken cancellationToken)
        {
            var validator = new CreateOrderItemCommandValidator();

            foreach (var orderItemCmd in orderItemsCmd)
            {
                var validationResult = await validator.ValidateAsync(orderItemCmd, cancellationToken);

                if (!validationResult.IsValid)
                    throw new ValidationException(validationResult.Errors);
            }
        }
    }
}