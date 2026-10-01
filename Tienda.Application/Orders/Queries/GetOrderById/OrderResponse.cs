namespace Tienda.Application.Orders.Queries.GetOrderById;

public record OrderItemResponse(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice
);

public record OrderResponse(
    Guid Id,
    string CustomerName,
    decimal TotalAmount,
    DateTime CreatedAt,
    IReadOnlyCollection<OrderItemResponse> Items
);