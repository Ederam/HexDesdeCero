namespace Tienda.Application.Orders.Commands.CreateOrder;

/// <summary>
/// DTO de entrada que representa el detalle de un ítem dentro de la orden a crear.
/// </summary>
/// <param name="ProductId">Identificador único del producto.</param>
/// <param name="Quantity">Cantidad solicitada del producto (debe ser mayor a 0).</param>
/// <param name="UnitPrice">Precio unitario del producto al momento de la compra.</param>
public sealed record CreateOrderItemCommand(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice
);