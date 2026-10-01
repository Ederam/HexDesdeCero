using MediatR;
using Tienda.Application.Orders.Queries.GetOrderById;
using Tienda.Domain.Entities;
using Tienda.Domain.Ports;

namespace Tienda.Application.Orders.Queries.GetOrderById;

/// <summary>
/// Manejador para procesar la consulta <see cref="GetOrderByIdQuery"/> y obtener los detalles de una orden específica.
/// </summary>
public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderResponse?>
{
    /// <summary>
    /// Puerto del repositorio de acceso a las órdenes.
    /// </summary>
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="GetOrderByIdQueryHandler"/>.
    /// </summary>
    /// <param name="orderRepository">Instancia del repositorio de órdenes inyectada mediante DI.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="orderRepository"/> es nulo.</exception>
    public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }

    /// <summary>
    /// Procesa la consulta para obtener una orden por su identificador único.
    /// </summary>
    /// <param name="request">Consulta que contiene el GUID de la orden.</param>
    /// <param name="cancellationToken">Token de cancelación de la tarea.</param>
    /// <returns>El DTO <see cref="OrderResponse"/> si la orden existe; de lo contrario, <c>null</c>.</returns>
    public async Task<OrderResponse?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        Order? order = await _orderRepository.GetByIdAsync(request.Id, cancellationToken);

        if (order is null)
        {
            return null;
        }

        List<OrderItemResponse> itemResponses = order.Items.Select((OrderItem item) => new OrderItemResponse(
            item.ProductId,
            item.ProductName,
            item.Quantity,
            item.UnitPrice,
            item.SubTotal
        )).ToList();

        return new OrderResponse(
            order.Id,
            order.CustomerId,
            order.Total,
            order.CreatedAt,
            itemResponses
        );
    }
}