using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Events;
using Ambev.DeveloperEvaluation.Application.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Ambev.DeveloperEvaluation.Application.Orders.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResult>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateOrderHandler> _logger;
        private readonly IEventPublisherService _eventPublisherService;

        private readonly OrderSpecification orderSpecification = new OrderSpecification();

        /// <summary>
        /// Initializes a new instance of CreateOrderHandler
        /// </summary>
        /// <param name="orderRepository">The Order repository</param>
        /// <param name="productRepository">The Product repository</param>
        /// <param name="customerRepository">The Customer repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public CreateOrderHandler(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            ICustomerRepository customerRepository,
            IBranchRepository branchRepository,
            IUserRepository userRepository,
            ILogger<CreateOrderHandler> logger,
            IMapper mapper,
            IEventPublisherService eventPublisherService)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _branchRepository = branchRepository;
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
            _eventPublisherService = eventPublisherService;
        }


        public async Task<Product> GetProduct(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new NotFoundException($"Product id {id} not found", null);

            return product;
        }

        public async Task ValidateDataRelations(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
            if (customer == null)
                throw new NotFoundException($"Customer id {request.CustomerId} not found", null);

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if (user == null)
                throw new NotFoundException($"User id {request.CustomerId} not found", null);

            var branch = await _branchRepository.GetByIdAsync(request.BranchId, cancellationToken);
            if (branch == null)
                throw new NotFoundException($"Branch id {request.CustomerId} not found", null);
        }


        public async Task<CreateOrderResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {

            try
            {
                _logger.LogInformation($"Receive request: {request}");

                var validator = new CreateOrderCommandValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                    throw new ValidationException(validationResult.Errors);

                await orderSpecification.ValidateOrderItems(request.OrderItems, cancellationToken);
                await ValidateDataRelations(request, cancellationToken);

                Order newOrder = _mapper.Map<Order>(request);
                newOrder.ChangeStatus(OrderStatus.Pending);

                var orderItems = _mapper.Map<IEnumerable<OrderItem>>(request.OrderItems);

                foreach (var orderItem in orderItems)
                {
                    var product = await GetProduct(orderItem.ProductId);

                    orderItem.UnitPrice = product.Price;
                    newOrder.AddItem(orderItem);
                }

                var orderResult = await _orderRepository.CreateAsync(newOrder, cancellationToken);

                // publish event
                await _eventPublisherService.PublishAsync(_mapper.Map<PackageOrderEvent>(newOrder), EventPublisherType.OrderCreated);

                return _mapper.Map<CreateOrderResult>(orderResult);
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ApplicationDomainException(ex.Message, ex);
            }            
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }
    }
}