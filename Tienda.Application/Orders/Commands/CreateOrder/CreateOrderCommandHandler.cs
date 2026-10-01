using MediatR;
using Tienda.Domain.Entities;
using Tienda.Domain.Ports;

namespace Tienda.Application.Orders.Commands.CreateOrder;

/// <summary>
/// Manejador de MediatR para orquestar la creación y persistencia de una orden.
/// </summary>
public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
{
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// Inicializa una nueva instancia del manejador con el puerto del repositorio de órdenes.
    /// </summary>
    /// <param name="orderRepository">Puerto de acceso a datos para órdenes.</param>
    public CreateOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }

    /// <summary>
    /// Procesa la solicitud, crea la entidad de dominio y la persiste de forma asíncrona.
    /// </summary>
    /// <param name="request">Comando con los datos de entrada para la nueva orden.</param>
    /// <param name="cancellationToken">Token de cancelación de la operación.</param>
    /// <returns>Un <see cref="CreateOrderResponse"/> con el resumen de la orden creada.</returns>
    public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // 1. Instanciar la entidad Order con tipo explícito
        Order order = new Order(request.CustomerName);

        // 2. Mapear y agregar cada ítem a la entidad de dominio
        foreach (CreateOrderItemCommand item in request.Items)
        {
            order.AddItem(
                item.ProductId,
                $"Producto-{item.ProductId}",
                item.Quantity,
                item.UnitPrice,
                0
            );
        }

        // 3. Persistir en el repositorio usando el método expuesto por IOrderRepository (SaveAsync / Add)
        await _orderRepository.SaveAsync(order, cancellationToken);

        // 4. Mapear y retornar la DTO de respuesta usando las propiedades del Dominio (Total / CreatedAt)
        return new CreateOrderResponse(
            order.Id,
            request.CustomerName,
            order.Total,
            order.CreatedAt
        );
    }
}