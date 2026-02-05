using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Events;
using Ambev.DeveloperEvaluation.Application.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Orders.UpdateOrder
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, UpdateOrderResult>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        private readonly IEventPublisherService _eventPublisherService;

        private readonly ILogger<UpdateOrderHandler> _logger;

        /// <summary>
        /// Initializes a new instance of UpdateOrderHandler
        /// </summary>
        /// <param name="orderRepository">The Order repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public UpdateOrderHandler(IOrderRepository orderRepository, ILogger<UpdateOrderHandler> logger, IMapper mapper, IEventPublisherService eventPublisherService)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
            _eventPublisherService = eventPublisherService;
        }

        public async Task<UpdateOrderResult> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Receive request: {request}");

                var order = await _orderRepository.GetByIdAsync(request.Id);

                if (order == null)
                    throw new NotFoundException($"Order id {request.Id} not Found", null);

                if (request.OrderDate.HasValue)
                    order.OrderDate = request.OrderDate.Value;

                if (!string.IsNullOrEmpty(request.Status))
                    order.ChangeStatus((OrderStatus)Enum.Parse(typeof(OrderStatus), request.Status));

                order = await _orderRepository.UpdateAsync(order, cancellationToken);

                // publish event
                await _eventPublisherService.PublishAsync(_mapper.Map<PackageOrderEvent>(order),
                    (order.Status == OrderStatus.Canceled)
                        ? EventPublisherType.OrderCanceled 
                        : EventPublisherType.OrderModified);

                return _mapper.Map<UpdateOrderResult>(order);

            }            
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}