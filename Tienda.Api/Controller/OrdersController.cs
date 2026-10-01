using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tienda.Application.Common.Models;
using Tienda.Application.Orders.Commands.CreateOrder;
using Tienda.Application.Orders.Queries.GetOrderById;
using Tienda.Application.Orders.Queries.GetOrders;

namespace Tienda.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ISender _mediator;

    public OrdersController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command, CancellationToken cancellationToken)
    {
        CreateOrderResponse response = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        OrderResponse? order = await _mediator.Send(new GetOrderByIdQuery(id), cancellationToken);

        if (order is null)
        {
            return NotFound(new { Message = $"No se encontró la orden con el ID '{id}'." });
        }

        return Ok(order);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<OrderResponse>>> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        GetOrdersQuery query = new GetOrdersQuery(pageNumber, pageSize);
        PagedResult<OrderResponse> result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}