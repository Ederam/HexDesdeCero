using MediatR;
using Tienda.Application.Common.Models;
using Tienda.Application.Orders.Queries.GetOrderById;
using Tienda.Domain.Entities;
using Tienda.Domain.Ports;

namespace Tienda.Application.Orders.Queries.GetOrders;

/// <summary>
/// Manejador de la consulta paginada <see cref="GetOrdersQuery"/>.
/// Encapsula la lógica de lectura paginada desde la capa de infraestructura hacia el DTO de respuesta.
/// </summary>
public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, PagedResult<OrderResponse>>
{
    /// <summary>
    /// Puerto de repositorio para el acceso a datos de las órdenes.
    /// </summary>
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="GetOrdersQueryHandler"/>.
    /// </summary>
    /// <param name="orderRepository">Instancia del repositorio de órdenes.</param>
    /// <exception cref="ArgumentNullException">Se lanza cuando el parámetro recibido es nulo.</exception>
    public GetOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }

    /// <summary>
    /// Ejecuta el caso de uso para obtener un listado paginado de órdenes.
    /// </summary>
    /// <param name="request">Objeto de consulta con los parámetros de paginación.</param>
    /// <param name="cancellationToken">Token para monitorear la cancelación de la operación.</param>
    /// <returns>Objeto paginado <see cref="PagedResult{OrderResponse}"/>.</returns>
    public async Task<PagedResult<OrderResponse>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        (IReadOnlyCollection<Order> items, int totalCount) pagedData = await _orderRepository.GetPagedAsync(
            request.PageNumber,
            request.PageSize,
            cancellationToken
        );

        List<OrderResponse> mappedOrders = pagedData.items.Select((Order order) => new OrderResponse(
            order.Id,
            order.CustomerId,
            order.Total,
            order.CreatedAt,
            order.Items.Select((OrderItem item) => new OrderItemResponse(
                item.ProductId,
                item.ProductName,
                item.Quantity,
                item.UnitPrice,
                item.SubTotal
            )).ToList()
        )).ToList();

        return new PagedResult<OrderResponse>(
            mappedOrders,
            request.PageNumber,
            request.PageSize,
            pagedData.totalCount
        );
    }
}