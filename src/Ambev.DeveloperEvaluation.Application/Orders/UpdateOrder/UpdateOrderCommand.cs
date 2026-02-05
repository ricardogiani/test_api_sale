using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Orders.UpdateOrder
{
    public class UpdateOrderCommand : IRequest<UpdateOrderResult>
    {
        public Guid Id { get; set; }

        public string Status { get; set; }

        public DateTime? OrderDate { get; set; }
        
        public override string ToString()
        {
            return $"Id: {Id}, Status: {Status}, OrderDate: {OrderDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? "null"}";
        }

    }
}