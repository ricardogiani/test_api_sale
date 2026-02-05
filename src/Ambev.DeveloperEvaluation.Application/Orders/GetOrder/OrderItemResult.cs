using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Orders.GetOrder
{
    public class OrderItemResult
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public long Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}