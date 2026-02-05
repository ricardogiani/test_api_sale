using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Orders.CreateOrder
{
    public class CreateOrderCommand : IRequest<CreateOrderResult>
    {
        public Guid CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        public Guid BranchId { get; set; }

        public IEnumerable<OrderItemCommand> OrderItems { get; set; }

        public Guid UserId { get; set; }

        public override string ToString()
        {
            return $"CustomerId: {CustomerId}, OrderDate: {OrderDate}, BranchId: {BranchId}," +
                $"UserId: {UserId}, ItemsCount: [{OrderItems?.Count()}]";
        }
    }
}