using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomer
{
    public class GetPaginatedCustomersHandler : IRequestHandler<GetPaginatedCustomersCommand, GetPaginatedCustomersResult>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of GetPaginatedProductsHandler
        /// </summary>
        /// <param name="customerRepository">The product repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="validator">The validator for CreateUserCommand</param>
        public GetPaginatedCustomersHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<GetPaginatedCustomersResult> Handle(GetPaginatedCustomersCommand request, CancellationToken cancellationToken)
        {
            // TODO validate
            
            (IEnumerable<Customer> Customers, int TotalCount) = await _customerRepository.GetPaginatedAsync(request.pageNumber, request.pageSize, cancellationToken);

            var response = new GetPaginatedCustomersResult()
            {
                CurrentPage = request.pageNumber,
                TotalCount = TotalCount,
                TotalPages = (TotalCount / request.pageSize),
                Customers = _mapper.Map<IEnumerable<GetCustomerResult>>(Customers)
            };

            return response;
        }
    }
}