using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    // TODO: nice to have
    public class GetPaginatedProductsHandler : IRequestHandler<GetPaginatedProductsCommand, GetPaginatedProductsResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of GetPaginatedProductsHandler
        /// </summary>
        /// <param name="productRepository">The product repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="validator">The validator for CreateUserCommand</param>
        public GetPaginatedProductsHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<GetPaginatedProductsResult> Handle(GetPaginatedProductsCommand request, CancellationToken cancellationToken)
        {
            // TODO: validator

            (IEnumerable<Product> Products, int TotalCount) = await _productRepository.GetPaginatedAsync(request.pageNumber, request.pageSize, cancellationToken);

            var response = new GetPaginatedProductsResult()
            {
                CurrentPage = request.pageNumber,
                TotalCount = TotalCount,
                TotalPages = (TotalCount / request.pageSize),
                Products = _mapper.Map<IEnumerable<GetProductResult>>(Products)
            };

            return response;
        }
    }
}