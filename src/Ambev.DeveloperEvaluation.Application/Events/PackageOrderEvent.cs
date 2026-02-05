using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Events
{
    public class PackageOrderEvent
    {
        public Guid Id { get; set; }
        
        public Guid OrderId { get; set; }

        public string Body { get; set; }
    }
}