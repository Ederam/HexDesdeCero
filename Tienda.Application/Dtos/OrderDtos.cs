namespace Tienda.Application.Dtos;

/// <summary>
/// Data Transfer Object representing an individual item line to be added to an order.
/// </summary>
/// <param name="ProductId">The unique identifier of the product.</param>
/// <param name="ProductName">The name of the product.</param>
/// <param name="Quantity">The requested quantity of the product.</param>
/// <param name="UnitPrice">The unit price of the product.</param>
/// <param name="AvailableStock">The current available inventory stock for validation.</param>
public record OrderItemDto(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    int AvailableStock
);

/// <summary>
/// Command DTO containing the necessary payload to execute the create order use case.
/// </summary>
/// <param name="CustomerId">The unique customer identifier.</param>
/// <param name="Items">The collection of order items to be added.</param>
public record CreateOrderDto(
    string CustomerId,
    List<OrderItemDto> Items
);

/// <summary>
/// Response DTO representing the resulting state of a successfully created order.
/// </summary>
/// <param name="Id">The unique order identifier.</param>
/// <param name="CustomerId">The customer identifier associated with the order.</param>
/// <param name="Status">The current status of the order.</param>
/// <param name="Total">The calculated total amount of the order.</param>
/// <param name="CreatedAt">The UTC creation timestamp.</param>
public record OrderResponseDto(
    Guid Id,
    string CustomerId,
    string Status,
    decimal Total,
    DateTime CreatedAt
);