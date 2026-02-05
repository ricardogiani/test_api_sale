using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Orders.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {            
            RuleFor(order => order.OrderItems).Must(items => items?.Any() == true).WithMessage("OrderItems must be greater than zero");
            RuleFor(order => order.CustomerId).NotEmpty();
            RuleFor(order => order.BranchId).NotEmpty();
            RuleFor(order => order.UserId).NotEmpty();
        }
    }
}