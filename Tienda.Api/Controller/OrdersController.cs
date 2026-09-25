using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tienda.Application.Dtos;
using Tienda.Application.Orders.CreateOrder;

namespace Tienda.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    [HttpPost]
    [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command, CancellationToken cancellationToken)
    {
        OrderResponseDto response = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(CreateOrder), new { id = response.Id }, response);
    }
}