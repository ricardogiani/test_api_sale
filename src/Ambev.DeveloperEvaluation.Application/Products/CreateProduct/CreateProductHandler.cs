using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResult>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of CreateProductHandler
        /// </summary>
        /// <param name="productRepository">The Product repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="validator">The validator for CreateProductCommand</param>
        public CreateProductHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // TODO:
            //var validator = new CreateUserCommandValidator();
            //var validationResult = await validator.ValidateAsync(command, cancellationToken);

            //if (!validationResult.IsValid)
            //throw new ValidationException(validationResult.Errors);

            //var existingUser = await _userRepository.GetByEmailAsync(command.Email, cancellationToken);
            //if (existingUser != null)
            //throw new InvalidOperationException($"User with email {command.Email} already exists");

            var product = _mapper.Map<Product>(request);
            

            var productCreated = await _productRepository.CreateAsync(product, cancellationToken);
            var result = _mapper.Map<CreateProductResult>(productCreated);
            return result;
        }
    }
}