using Microsoft.AspNetCore.Mvc;
using Tienda.Application.Dtos;
using Tienda.Application.UseCases;
using Tienda.Domain.Exceptions;

namespace Tienda.Api.Controllers;

/// <summary>
/// Primary REST API adapter exposing endpoints for Order management workflows.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly CreateOrderUseCase _createOrderUseCase;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrdersController"/> class.
    /// </summary>
    /// <param name="createOrderUseCase">The order creation application service dependency.</param>
    public OrdersController(CreateOrderUseCase createOrderUseCase)
    {
        _createOrderUseCase = createOrderUseCase ?? throw new ArgumentNullException(nameof(createOrderUseCase));
    }

    /// <summary>
    /// Creates and confirms a new order line request.
    /// </summary>
    /// <param name="command">The creation payload.</param>
    /// <param name="cancellationToken">Cancellation token signal.</param>
    /// <returns>The created order details.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto command, CancellationToken cancellationToken)
    {
        try
        {
            OrderResponseDto response = await _createOrderUseCase.ExecuteAsync(command, cancellationToken);
            return CreatedAtAction(nameof(CreateOrder), new { id = response.Id }, response);
        }
        catch (InsufficientStockException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}