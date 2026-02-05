using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class OrderItem
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }

        public long Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Discount { get; set; }

        public decimal TotalAmount { get; set; }

        public void ApplyValues()
        {
            if (Quantity > DomainSettings.Discount20_Percent.TotalItems)
                Discount = UnitPrice * DomainSettings.Discount20_Percent.Discount;
            else if (Quantity > DomainSettings.Discount10_Percent.TotalItems)
                Discount = UnitPrice * DomainSettings.Discount10_Percent.Discount;
            else
                Discount = UnitPrice * DomainSettings.NothingDiscount;

            TotalAmount = (UnitPrice - Discount) * Quantity;
        }
        
        public override string ToString()
        {
            return $"OrderId: {OrderId}, ProductId: {ProductId}, Quantity: {Quantity}, " +
                $"UnitPrice: {UnitPrice:F2}, Discount: {Discount:F2}, TotalAmount: {TotalAmount:F2}";
        }

    }


}