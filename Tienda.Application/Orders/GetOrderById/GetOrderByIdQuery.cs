using MediatR;
using Tienda.Application.Dtos;

namespace Tienda.Application.Orders.GetOrderById;

/// <summary>
/// Consulta inmutable para obtener los detalles de una orden por su identificador único.
/// </summary>
public record GetOrderByIdQuery(Guid Id) : IRequest<OrderResponseDto>;