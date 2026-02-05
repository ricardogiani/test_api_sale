using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    public interface IBranchRepository
    {
        /// <summary>
        /// Retrieves a branch by their unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the branches</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The branch if found, null otherwise</returns>
        Task<Branch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}