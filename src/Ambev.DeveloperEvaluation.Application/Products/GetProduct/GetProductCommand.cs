using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct
{
    public class GetProductCommand  : IRequest<GetProductResult>
    {
        public Guid Id { get; set; }

        public GetProductCommand(Guid id)
        {
            Id = id;
        }
    }
}