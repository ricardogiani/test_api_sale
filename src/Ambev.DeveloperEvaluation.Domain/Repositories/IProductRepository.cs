using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface IProductRepository
    {
        /// <summary>
        /// Creates a new Product in the repository
        /// </summary>
        /// <param name="product">The user to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created user</returns>
        Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update a Product from the repository
        /// </summary>
        /// <param name="product">Product to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if the user was deleted, false if not found</returns>
        Task<Product?> UpdateAsync(Product existProduct, Product productValues, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a Product by their unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the user</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The user if found, null otherwise</returns>
        Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves products based on a filter function
        /// </summary>
        /// <param name="filter">The filter function to apply</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A collection of products that match the filter</returns>
        Task<IEnumerable<Product?>> GetByFilter(Func<Product, bool> filter, CancellationToken cancellationToken = default);

        /// <summary>
        ///  Get paginated products
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns> <summary>
        Task<(IEnumerable<Product> Products, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);


    }
}