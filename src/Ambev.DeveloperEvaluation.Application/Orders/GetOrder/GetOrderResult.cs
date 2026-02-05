using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Branches;
using Ambev.DeveloperEvaluation.Application.Customers.GetCustomer;

namespace Ambev.DeveloperEvaluation.Application.Orders.GetOrder
{
    public class GetOrderResult
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public GetCustomerResult Customer { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public BranchResult Branch { get; set; }        

        public string Status { get; set; }

        public IReadOnlyCollection<OrderItemResult> OrderItems { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}