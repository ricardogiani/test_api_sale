using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct
{
    public class GetProductRequest
    {
        private Guid id;

        public GetProductRequest(Guid id)
        {
            this.id = id;
        }
    }
}