using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of BranchRepository
        /// </summary>
        /// <param name="context">The database context</param>
        public BranchRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Branch?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var branch = await _context.Branches.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return branch;
        }
    }
}