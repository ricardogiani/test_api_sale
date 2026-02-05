using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Customers.GetCustomer
{
    public class GetPaginatedCustomersCommand : IRequest<GetPaginatedCustomersResult>
    {
        public int pageNumber;
        public int pageSize;

        public GetPaginatedCustomersCommand(int pageNumber, int pageSize)
        {
            this.pageNumber = pageNumber;
            this.pageSize = pageSize;
        }
    }
}