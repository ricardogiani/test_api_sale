using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Orders.CreateOrder
{
    public class CreateOrderItemCommandValidator : AbstractValidator<OrderItemCommand>
    {
        public CreateOrderItemCommandValidator()
        {
            RuleFor(item => item.ProductId)
                .NotEmpty()
                .WithMessage("ProductId is required.");

            RuleFor(item => item.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");
     
        }
    }
}