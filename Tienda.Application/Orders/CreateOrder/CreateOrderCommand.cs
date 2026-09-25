using MediatR;
using Tienda.Application.Dtos;

namespace Tienda.Application.Orders.CreateOrder;

public record CreateOrderItemCommand(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    int AvailableStock
);

public record CreateOrderCommand(
    string CustomerId,
    List<CreateOrderItemCommand> Items
) : IRequest<OrderResponseDto>;