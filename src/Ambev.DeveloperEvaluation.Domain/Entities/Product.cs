using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Product : BaseEntity
    {
        /// <summary>
        /// The name of the product
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The product description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The product category
        /// </summary>
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// The product brand
        /// </summary>
        public string Brand { get; set; } = string.Empty;

        /// <summary>
        /// The unit price of the product
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The current stock quantity
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// Indicates if the product is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// The date when the product was created
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The date when the product was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// The minimum stock level for alerts
        /// </summary>
        public int MinimumStockLevel { get; set; } = 0;

        /// <summary>
        /// Product image URL
        /// </summary>
        public string? ImageUrl { get; set; }
    }
}