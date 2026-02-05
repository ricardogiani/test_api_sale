using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.WebApi.Features.Branches;
using Ambev.DeveloperEvaluation.WebApi.Features.Customers;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders.GetOrder
{
    public class GetOrderResponse
    {
        public Guid Id { get; set; }

        public CustomerResponse Customer { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public BranchResponse Branch { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
        
        public IEnumerable<OrderItemResponse> OrderItems { get; set; }
    }
}