using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of ProductsController
        /// </summary>
        /// <param name="mediator">The mediator instance</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public ProductsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

       /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>The product details if found.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var query = new GetProductCommand(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound(new ApiResponse
                {
                    Success = false,
                    Message = "Product not found"
                });
            }

            return Ok(new ApiResponseWithData<CreateProductResponse>
            {
                Success = true,
                Message = "Product retrieved successfully",
                Data = _mapper.Map<CreateProductResponse>(result)
            });
        }

        /// <summary>
        /// Retrieves paginated products.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>A paginated list of products.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseWithData<PaginatedResponse<ProductResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPaginated([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetPaginatedProductsCommand(pageNumber, pageSize);
            var result = await _mediator.Send(query);
            var paginatedList = new PaginatedList<ProductResponse>(_mapper.Map<IEnumerable<ProductResponse>>(result.Products), result.TotalCount, result.CurrentPage, pageSize);
            return OkPaginated(paginatedList);
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="request">The product creation command.</param>
        /// <returns>The created product.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateProductResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Invalid request data"
                });

            var command = _mapper.Map<CreateProductCommand>(request);

            var result = await _mediator.Send(command);
            return Created(string.Empty, new ApiResponseWithData<CreateProductResponse>
            {
                Success = true,
                Message = "Product created successfully",
                Data = _mapper.Map<CreateProductResponse>(result)
            });
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="request">The product update command.</param>
        /// <returns>No content if successful.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Invalid request data"
                });

            if (id != request.Id)
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Product ID mismatch"
                });

            var command = _mapper.Map<UpdateProductCommand>(request);

            var result = await _mediator.Send(command);

            return Ok(new ApiResponseWithData<CreateProductResponse>
            {
                Success = true,
                Message = "Product updated successfully",
                Data = _mapper.Map<CreateProductResponse>(result)
            });
        }

        
    }
}