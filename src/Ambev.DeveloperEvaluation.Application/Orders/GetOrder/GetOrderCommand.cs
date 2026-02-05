using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Orders.GetOrder
{
    public class GetOrderCommand : IRequest<GetOrderResult>
    {
        public Guid Id { get; set; }

        public GetOrderCommand(Guid id)
        {
            Id = id;
        }
    }
}