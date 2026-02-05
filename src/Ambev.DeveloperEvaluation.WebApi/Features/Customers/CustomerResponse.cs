using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Customers
{
    public class CustomerResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Document { get; set; }

        public string Email { get; set; }
        
        public string Phone { get; set; }
    }
}