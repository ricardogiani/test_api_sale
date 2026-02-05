using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Orders.GetOrder
{
    public class GetOrderHandler : IRequestHandler<GetOrderCommand, GetOrderResult>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of GetOrderHandler
        /// </summary>
        /// <param name="orderRepository">The Order repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public GetOrderHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<GetOrderResult> Handle(GetOrderCommand request, CancellationToken cancellationToken)
        {
            var result = await _orderRepository.GetByIdWithIncludesAsync(request.Id, cancellationToken, x => x.OrderItems, x => x.Branch, x => x.Customer, x => x.User);

            return _mapper.Map<GetOrderResult>(result);
        }
    }
}