using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface IOrderRepository
    {
        /// <summary>
        /// Creates a new Order in the repository
        /// </summary>
        /// <param name="order">The order to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created order</returns>
        Task<Order> CreateAsync(Order order, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update an Order from the repository
        /// </summary>
        /// <param name="order">Order to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated order</returns>
        Task<Order> UpdateAsync(Order existOrder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves an Order by its unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the order</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The order if found, null otherwise</returns>
        Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<Order?> GetByIdWithIncludesAsync(Guid id, CancellationToken cancellationToken, params Expression<Func<Order, object>>[] includes);
    }
}