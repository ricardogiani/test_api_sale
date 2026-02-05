using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Events;
using Ambev.DeveloperEvaluation.Application.Exceptions;
using Ambev.DeveloperEvaluation.Application.Orders;
using Ambev.DeveloperEvaluation.Application.Orders.CreateOrder;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Tests.Application.Orders
{
    public class CreateOrderHandlerTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly Mock<IBranchRepository> _mockBranchRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<CreateOrderHandler>> _mockLogger;
        private readonly Mock<IEventPublisherService> _mockEventPublisherService;
        
        private readonly CreateOrderHandler _handler;
        private readonly Faker _faker;
        private readonly Faker<Product> _productFaker;
        private readonly Faker<Customer> _customerFaker;
        private readonly Faker<User> _userFaker;
        private readonly Faker<Branch> _branchFaker;
        private readonly Faker<OrderItemCommand> _orderItemCommandFaker;

        public CreateOrderHandlerTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockBranchRepository = new Mock<IBranchRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<CreateOrderHandler>>();
            _mockEventPublisherService = new Mock<IEventPublisherService>();

            _handler = new CreateOrderHandler(
                _mockOrderRepository.Object,
                _mockProductRepository.Object,
                _mockCustomerRepository.Object,
                _mockBranchRepository.Object,
                _mockUserRepository.Object,
                _mockLogger.Object,
                _mockMapper.Object,
                _mockEventPublisherService.Object);
            
            _faker = new Faker("pt_BR");
            
            // Configuração dos fakers para cada entidade
            _productFaker = new Faker<Product>("pt_BR")
                .RuleFor(p => p.Id, f => Guid.NewGuid())
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Price, f => f.Random.Decimal(10, 1000))
                .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
                .RuleFor(p => p.Brand, f => f.Company.CompanyName())
                .RuleFor(p => p.StockQuantity, f => f.Random.Int(10, 500))
                .RuleFor(p => p.MinimumStockLevel, f => f.Random.Int(1, 10))
                .RuleFor(p => p.ImageUrl, f => f.Image.PicsumUrl())
                .RuleFor(p => p.IsActive, f => true);
            
            _customerFaker = new Faker<Customer>("pt_BR")
                .RuleFor(c => c.Id, f => Guid.NewGuid())
                .RuleFor(c => c.Name, f => f.Person.FullName)
                .RuleFor(c => c.Document, f => f.Finance.Account(11))
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber("(##) #####-####"));
            
            _userFaker = new Faker<User>("pt_BR")
                .RuleFor(u => u.Id, f => Guid.NewGuid())
                .RuleFor(u => u.Username, f => f.Person.FullName)
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Password, f => f.Internet.Password());
            
            _branchFaker = new Faker<Branch>("pt_BR")
                .RuleFor(b => b.Id, f => Guid.NewGuid())
                .RuleFor(b => b.Name, f => f.Company.CompanyName())
                .RuleFor(b => b.City, f => f.Address.City())
                .RuleFor(b => b.Country, f => f.Address.Country());
            
            _orderItemCommandFaker = new Faker<OrderItemCommand>()
                .RuleFor(i => i.ProductId, f => Guid.NewGuid())
                .RuleFor(i => i.Quantity, f => f.Random.Int(1, 10));
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsCreatedOrder()
        {
            // Arrange
            var customer = _customerFaker.Generate();
            var user = _userFaker.Generate();
            var branch = _branchFaker.Generate();
            var product = _productFaker.Generate();
            
            var orderItems = new List<OrderItemCommand>
            {
                new OrderItemCommand 
                { 
                    ProductId = product.Id, 
                    Quantity = _faker.Random.Int(1, 10)                    
                }
            };
            
            var command = new CreateOrderCommand
            {
                CustomerId = customer.Id,
                UserId = user.Id,
                BranchId = branch.Id,
                OrderDate = _faker.Date.Recent(7),
                OrderItems = orderItems
            };
            
            var mappedOrder = new Order
            {
                CustomerId = customer.Id,
                UserId = user.Id,
                BranchId = branch.Id,
                OrderDate = command.OrderDate
            };
            
            var mappedOrderItems = orderItems.Select(oi => new OrderItem 
            { 
                ProductId = oi.ProductId, 
                Quantity = oi.Quantity,
                UnitPrice = product.Price
            }).ToList();
            
            var createdOrder = new Order 
            { 
                Id = Guid.NewGuid(),
                CustomerId = customer.Id,
                UserId = user.Id,
                BranchId = branch.Id,
                OrderDate = command.OrderDate
            };
            
            var expectedResult = new CreateOrderResult { Id = createdOrder.Id };
            
            // Setup validation to pass
            _mockCustomerRepository.Setup(repo => repo.GetByIdAsync(customer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);
            
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
                
            _mockBranchRepository.Setup(repo => repo.GetByIdAsync(branch.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(branch);
                
            _mockProductRepository.Setup(repo => repo.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);
            
            _mockMapper.Setup(m => m.Map<Order>(command)).Returns(mappedOrder);
            _mockMapper.Setup(m => m.Map<IEnumerable<OrderItem>>(command.OrderItems))
                .Returns(mappedOrderItems);
            _mockOrderRepository.Setup(repo => repo.CreateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdOrder);
            _mockMapper.Setup(m => m.Map<CreateOrderResult>(createdOrder))
                .Returns(expectedResult);
            
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Id, result.Id);
            _mockEventPublisherService.Verify(service => 
                service.PublishAsync(It.IsAny<PackageOrderEvent>(), EventPublisherType.OrderCreated), 
                Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var command = new CreateOrderCommand();
            
            // Act & Assert
            await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => 
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_CustomerNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            
            var command = new CreateOrderCommand
            {
                CustomerId = customerId,
                UserId = Guid.NewGuid(),
                BranchId = Guid.NewGuid(),
                OrderDate = _faker.Date.Recent(7),
                OrderItems = new List<OrderItemCommand>
                {
                    _orderItemCommandFaker.Generate()
                }
            };
            
            _mockCustomerRepository.Setup(repo => repo.GetByIdAsync(customerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Customer)null);
                
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => 
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_UserNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var customer = _customerFaker.Generate();
            var userId = Guid.NewGuid();
            
            var command = new CreateOrderCommand
            {
                CustomerId = customer.Id,
                UserId = userId,
                BranchId = Guid.NewGuid(),
                OrderDate = _faker.Date.Recent(7),                
                OrderItems = new List<OrderItemCommand>
                {
                    _orderItemCommandFaker.Generate()
                }
            };

            var mappedOrder = new Order
            {
                CustomerId = customer.Id,
                UserId = Guid.NewGuid(),
                BranchId = Guid.NewGuid(),
                OrderDate = command.OrderDate
            };

            _mockMapper.Setup(m => m.Map<Order>(command)).Returns(mappedOrder);
            
            _mockCustomerRepository.Setup(repo => repo.GetByIdAsync(customer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);
                
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((User)null);
                
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => 
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ProductNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var customer = _customerFaker.Generate();
            var user = _userFaker.Generate();
            var branch = _branchFaker.Generate();
            var productId = Guid.NewGuid();
            
            var orderItems = new List<OrderItemCommand>
            {
                new OrderItemCommand 
                { 
                    ProductId = productId, 
                    Quantity = _faker.Random.Int(1, 10)                    
                }
            };
            
            var command = new CreateOrderCommand
            {
                CustomerId = customer.Id,
                UserId = user.Id,
                BranchId = branch.Id,
                OrderDate = _faker.Date.Recent(7),
                OrderItems = orderItems
            };
            
            var mappedOrder = new Order();
            var mappedOrderItems = orderItems.Select(oi => new OrderItem 
            { 
                ProductId = oi.ProductId, 
                Quantity = oi.Quantity
            }).ToList();
            
            _mockCustomerRepository.Setup(repo => repo.GetByIdAsync(customer.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(customer);
                
            _mockUserRepository.Setup(repo => repo.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
                
            _mockBranchRepository.Setup(repo => repo.GetByIdAsync(branch.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(branch);
                
            _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);
                
            _mockMapper.Setup(m => m.Map<Order>(command)).Returns(mappedOrder);
            _mockMapper.Setup(m => m.Map<IEnumerable<OrderItem>>(command.OrderItems))
                .Returns(mappedOrderItems);
                
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => 
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task GetProduct_ProductExists_ReturnsProduct()
        {
            // Arrange
            var product = _productFaker.Generate();
            
            _mockProductRepository.Setup(repo => repo.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);
                
            // Act
            var result = await _handler.GetProduct(product.Id);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);
            Assert.Equal(product.Price, result.Price);
        }

        [Fact]
        public async Task GetProduct_ProductNotExists_ThrowsNotFoundException()
        {
            // Arrange
            var productId = Guid.NewGuid();
            
            _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);
                
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => 
                _handler.GetProduct(productId));
        }
    }
}