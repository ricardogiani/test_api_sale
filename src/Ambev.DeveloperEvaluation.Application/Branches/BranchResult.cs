using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Branches
{
    public class BranchResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Country { get; set; }

        public string City { get; set; }
    }
}