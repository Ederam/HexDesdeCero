using MediatR;
using Tienda.Application.Common.Models;
using Tienda.Application.Orders.Queries.GetOrderById;

namespace Tienda.Application.Orders.Queries.GetOrders;

/// <summary>
/// Representa la consulta paginada para obtener un listado de órdenes de compra.
/// </summary>
/// <param name="PageNumber">Número de la página a consultar (por defecto 1).</param>
/// <param name="PageSize">Cantidad de elementos a retornar por página (por defecto 10).</param>
public record GetOrdersQuery(
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<PagedResult<OrderResponse>>;