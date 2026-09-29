using MediatR;
using Tienda.Application.Dtos;
using Tienda.Domain.Entities;
using Tienda.Domain.Exceptions;
using Tienda.Domain.Ports;

namespace Tienda.Application.Orders.GetOrderById;

/// <summary>
/// Manejador encargado de procesar la consulta <see cref="GetOrderByIdQuery"/> y proyectar la entidad de Dominio a un DTO de respuesta.
/// </summary>
public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderResponseDto>
{
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// Inicializa una nueva instancia del manejador <see cref="GetOrderByIdQueryHandler"/>.
    /// </summary>
    /// <param name="orderRepository">Puerto del repositorio de órdenes para acceder a la persistencia.</param>
    public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }

    /// <summary>
    /// Procesa la consulta para buscar una orden por su ID en el repositorio.
    /// </summary>
    /// <param name="request">Instancia de la consulta que contiene el identificador de la orden.</param>
    /// <param name="cancellationToken">Token de cancelación de la operación asíncrona.</param>
    /// <returns>Retorna el DTO <see cref="OrderResponseDto"/> con los detalles de la orden.</returns>
    /// <exception cref="OrderNotFoundException">Se lanza cuando no se encuentra ninguna orden con el ID proporcionado.</exception>
    public async Task<OrderResponseDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        Order? order = await _orderRepository.GetByIdAsync(request.Id);

        if (order is null)
        {
            throw new OrderNotFoundException(request.Id);
        }

        List<OrderItemResponseDto> itemDtos = order.Items
            .Select(i => new OrderItemResponseDto(
                i.ProductId,
                i.ProductName,
                i.Quantity,
                i.UnitPrice,
                i.Quantity * i.UnitPrice))
            .ToList();

        return new OrderResponseDto(
            order.Id,
            order.CustomerId,
            order.Status.ToString(),
            order.Total,
            order.CreatedAt,
            itemDtos
        );
    }
}