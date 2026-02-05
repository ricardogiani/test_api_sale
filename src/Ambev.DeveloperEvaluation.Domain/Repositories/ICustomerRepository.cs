using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> CreateAsync(Customer customer, CancellationToken cancellationToken = default);

        /// <summary>
        ///  Get paginated customers
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns> <summary>
        Task<(IEnumerable<Customer> Products, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a customers by their unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the customers</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The customers if found, null otherwise</returns>
        Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}