using MediatR;

namespace Tienda.Application.Orders.Commands.CreateOrder;

/// <summary>
/// Comando CQRS para solicitar la creación de una nueva orden en el sistema.
/// </summary>

/// <param name="CustomerName">Nombre completo del cliente que realiza la compra.</param>
/// <param name="Items">Lista de ítems o productos incluidos en la orden.</param>
public sealed record CreateOrderCommand(
    string CustomerName,
    List<CreateOrderItemCommand> Items
) : IRequest<CreateOrderResponse>;