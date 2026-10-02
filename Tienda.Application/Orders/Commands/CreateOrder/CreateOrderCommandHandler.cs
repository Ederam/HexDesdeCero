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
        // 1. Crear la orden usando el constructor de nacimiento
        Order order = new Order(request.CustomerId);

        // 2. Agregar los ítems usando el tipo explícito CreateOrderItemCommand
        foreach (CreateOrderItemCommand item in request.Items)
        {
            // Simulación de stock disponible mientras no exista la consulta a IProductRepository
            int simulatedAvailableStock = int.MaxValue;

            order.AddItem(
                item.ProductId,
                item.ProductName,
                item.Quantity,
                item.UnitPrice,
                simulatedAvailableStock);
        }

        // 3. Guardar en el repositorio
        await _orderRepository.SaveAsync(order, cancellationToken);

        // 4. Retornar la respuesta encapsulated en CreateOrderResponse
        return new CreateOrderResponse(order.Id);
    }
}