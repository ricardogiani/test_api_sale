using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of ProductRepository
        /// </summary>
        /// <param name="context">The database context</param>
        public ProductRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
        {
            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return product;
        }

        public async Task<IEnumerable<Product?>> GetByFilter(Func<Product, bool> filter, CancellationToken cancellationToken = default)
        {
            return await _context.Products.Where(x => filter(x)).ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Products.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<Product?> UpdateAsync(Product existProduct, Product productValues, CancellationToken cancellationToken = default)
        {
            _context.Entry(existProduct).CurrentValues.SetValues(productValues);
            await _context.SaveChangesAsync(cancellationToken);
            return productValues;
        }
        
        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var totalCount = await _context.Products.CountAsync(cancellationToken);
            var products = await _context.Products
                                         .Skip((pageNumber - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToListAsync(cancellationToken);

            return (products, totalCount);
        }
    }
}