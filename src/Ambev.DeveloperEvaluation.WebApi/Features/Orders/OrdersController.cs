using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Exceptions;
using Ambev.DeveloperEvaluation.Application.Orders.CreateOrder;
using Ambev.DeveloperEvaluation.Application.Orders.CreateOrderItem;
using Ambev.DeveloperEvaluation.Application.Orders.DeleteOrderItem;
using Ambev.DeveloperEvaluation.Application.Orders.GetOrder;
using Ambev.DeveloperEvaluation.Application.Orders.UpdateOrder;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Orders.CreateOrder;
using Ambev.DeveloperEvaluation.WebApi.Features.Orders.CreateOrderItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Orders.GetOrder;
using Ambev.DeveloperEvaluation.WebApi.Features.Orders.UpdateOrder;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Orders
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : BaseController
    {

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<OrdersController> _logger;

        /// <summary>
        /// Initializes a new instance of OrdersController
        /// </summary>
        /// <param name="mediator">The mediator instance</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public OrdersController(IMediator mediator, IMapper mapper, ILogger<OrdersController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves an order by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the order.</param>
        /// <returns>The order details if found.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetOrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var command = new GetOrderCommand(id);
                var result = await _mediator.Send(command);

                if (result == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Order not found"
                    });
                }

                return Ok(_mapper.Map<GetOrderResponse>(result));
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="request">The order creation request.</param>
        /// <returns>The created order.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateOrderResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid request data"
                    });

                var command = _mapper.Map<CreateOrderCommand>(request);

                var result = await _mediator.Send(command);

                // TODO BadRequest, validation, logs
                return Created(string.Empty, new ApiResponseWithData<CreateOrderResponse>
                {
                    Success = true,
                    Message = "Order created successfully",
                    Data = _mapper.Map<CreateOrderResponse>(result)
                });
            }
            catch (ApplicationDomainException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

         /// <summary>
        /// Partially updates an order.
        /// </summary>
        /// <param name="id">The unique identifier of the order.</param>
        /// <param name="request">The partial update request.</param>
        /// <returns>The updated order.</returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(ApiResponseWithData<UpdateOrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePartial(Guid id, [FromBody] UpdateOrderRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid request data"
                    });

                var command = _mapper.Map<UpdateOrderCommand>(request);
                command.Id = id;

                var result = await _mediator.Send(command);

                if (result == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Success = false,
                        Message = "Order not found"
                    });
                }

                return Ok(new ApiResponseWithData<UpdateOrderResponse>
                {
                    Success = true,
                    Message = "Order updated successfully",
                    Data = _mapper.Map<UpdateOrderResponse>(result)
                });
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Add Order Items 
        /// </summary>
        /// <param name="request">The order creation request.</param>
        /// <returns>The created order.</returns>
        [HttpPost("{id}/items")]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateOrderResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrderItems(Guid id, [FromBody] CreateOrderItemRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Invalid request data"
                    });

                var command = _mapper.Map<CreateOrderItemCommand>(request);

                var result = await _mediator.Send(command);
                return Created(string.Empty, _mapper.Map<CreateOrderItemResponse>(result));    
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }

        /// <summary>
        /// Deletes an order item by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the order.</param>
        /// <param name="idProduct">The unique identifier of the product.</param>
        /// <returns>No content if the deletion is successful.</returns>
        [HttpDelete("{id}/items/{idProduct}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteOrderItem(Guid id, Guid idProduct)
        {
            try
            {
                var command = new DeleteOrderItemCommand(id, idProduct);

                await _mediator.Send(command);              

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }       
    }
}