using MediatR;

namespace Tienda.Application.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(Guid Id) : IRequest<OrderResponse?>;