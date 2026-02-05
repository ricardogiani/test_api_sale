using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of CustomerRepository
        /// </summary>
        /// <param name="context">The database context</param>
        public CustomerRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Customer> CreateAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            await _context.Customers.AddAsync(customer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return customer;
        }

        public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return customer;
        }

        public async Task<(IEnumerable<Customer> Products, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var totalCount = await _context.Customers.CountAsync(cancellationToken);
            var customers = await _context.Customers
                                         .Skip((pageNumber - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToListAsync(cancellationToken);

            return (customers, totalCount);
        }
    }
}