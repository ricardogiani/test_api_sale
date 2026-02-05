using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of ProductRepository
        /// </summary>
        /// <param name="context">The database context</param>
        public OrderRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _context.Orders.AddAsync(order, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return order;
        }

        public async Task<Order> UpdateAsync(Order order, CancellationToken cancellationToken = default)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync(cancellationToken);
            return order;            
        }

        public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return order;
        }

        public async Task<Order?> GetByIdWithIncludesAsync(Guid id, CancellationToken cancellationToken, params Expression<Func<Order, object>>[] includes)
        {
            var query = _context.Orders.AsQueryable();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var order = await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return order;
        }
    }
}