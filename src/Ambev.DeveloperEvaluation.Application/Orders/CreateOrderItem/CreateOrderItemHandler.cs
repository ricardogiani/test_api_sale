using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Orders.CreateOrderItem
{
    public class CreateOrderItemHandler : IRequestHandler<CreateOrderItemCommand, CreateOrderItemResult>
    {
        private readonly ILogger<CreateOrderItemHandler> _logger;

        private readonly IOrderRepository _orderRepository;

        private readonly IProductRepository _productRepository;

        private readonly IMapper _mapper;

        private readonly OrderSpecification orderSpecification = new OrderSpecification();

        public CreateOrderItemHandler(ILogger<CreateOrderItemHandler> logger, IOrderRepository orderRepository, IProductRepository productRepository, IMapper mapper)
        {
            _logger = logger;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Product> GetProduct(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new NotFoundException($"Product id {id} not found", null);

            return product;
        }

        public async Task<CreateOrderItemResult> Handle(CreateOrderItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Receive request: {request}");

                var order = await _orderRepository.GetByIdWithIncludesAsync(request.OrderId, cancellationToken, x => x.OrderItems);
                if (order == null)
                    throw new NotFoundException($"Order id {request.OrderId} not Found", null);

                await orderSpecification.ValidateOrderItems(request.OrderItems, cancellationToken);

                var orderItems = _mapper.Map<IEnumerable<OrderItem>>(request.OrderItems);

                foreach (var orderItem in orderItems)
                {
                    var product = await GetProduct(orderItem.ProductId);

                    orderItem.UnitPrice = product.Price;
                    order.AddItem(orderItem);
                }

                await _orderRepository.UpdateAsync(order);

                return new CreateOrderItemResult() { OrderId = request.OrderId };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}