using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Order : BaseEntity
    {

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public Guid BranchId { get; set; }
        public Branch Branch { get; set; }

        private OrderStatus _status;
        public OrderStatus Status => _status;

        private readonly List<OrderItem> _orderItems = new();

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        /// <summary>
        /// User Id Operator
        /// </summary>
        /// <value></value>
        public Guid UserId { get; set; }
        public User User { get; set; }

        /// <summary>
        /// Gets the date and time when the order was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets the date and time of the last update order
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public void ChangeStatus(OrderStatus newStatus)
        {
            if (Status == OrderStatus.Completed || Status == OrderStatus.Canceled)
                throw new DomainException($"Order id {Id} with status {Status} does not allow changes status");

            _status = newStatus;
        }

        public (bool Success, string Message) AddItem(OrderItem orderItem)
        {
            if (!(Status == OrderStatus.Pending))
                throw new DomainException($"Order id {Id} does not allow changes to items");

            var itemExist = _orderItems.FirstOrDefault(x => x.ProductId == orderItem.ProductId);

            if (itemExist != null)
                itemExist.Quantity += orderItem.Quantity;
            else
            {
                _orderItems.Add(orderItem);
                itemExist = orderItem;
            }

            if (itemExist.Quantity > DomainSettings.MaxSameItemsToOrder)
                throw new DomainException($"It's not possible to sell above {DomainSettings.MaxSameItemsToOrder} identical items. productId: {itemExist.ProductId}");

            itemExist.ApplyValues();

            TotalAmount = _orderItems.Sum(x => x.TotalAmount);

            return (Success: true, Message: "Success");
        }

        public bool RemoveItem(OrderItem item)
        {
            if (!(Status == OrderStatus.Pending))
                throw new DomainException($"Order id {Id} does not allow changes to items");

            var count = _orderItems.RemoveAll(x => x.ProductId == item.ProductId);

            TotalAmount = _orderItems.Sum(x => x.TotalAmount);

            return (count > 0);
        }


        public override string ToString()
        {
            return $"Id: {Id}, CustomerId: {CustomerId}, OrderDate: {OrderDate:yyyy-MM-dd HH:mm:ss}, " +
                $"TotalAmount: {TotalAmount:F2}, Status: {Status}, BranchId: {BranchId}, " +
                $"UserId: {UserId}, Items: {OrderItems.Count}, CreatedAt: {CreatedAt:yyyy-MM-dd HH:mm:ss}";
        }
        

    }
}