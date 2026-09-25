namespace Tienda.Application.Dtos;

/// <summary>
/// DTO que representa un ítem individual de una orden.
/// </summary>
public record OrderItemDto(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    int AvailableStock
);

/// <summary>
/// DTO de respuesta que representa la línea de detalle de una orden procesada.
/// </summary>
public record OrderItemResponseDto(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal Subtotal
);

/// <summary>
/// DTO de respuesta que representa el estado final de una orden creada.
/// </summary>
public record OrderResponseDto(
    Guid Id,
    string CustomerId,
    string Status,
    decimal Total,
    DateTime CreatedAt,
    List<OrderItemResponseDto> Items
);