using Tienda.Application.Dtos;
using Tienda.Domain.Entities;
using Tienda.Domain.Ports;

namespace Tienda.Application.UseCases;

/// <summary>
/// Application service/use case that orchestrates the order creation workflow.
/// </summary>
public class CreateOrderUseCase
{
    private readonly IOrderRepository _orderRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateOrderUseCase"/> class.
    /// </summary>
    /// <param name="orderRepository">The order repository port dependency.</param>
    public CreateOrderUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }

    /// <summary>
    /// Executes the business workflow to build, validate, confirm, and persist a new order.
    /// </summary>
    /// <param name="command">The payload containing order creation parameters.</param>
    /// <param name="cancellationToken">Cancellation token signal.</param>
    /// <returns>A response DTO representing the persisted order.</returns>
    public async Task<OrderResponseDto> ExecuteAsync(CreateOrderDto command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        // 1. Create the Aggregate Root
        Order order = new Order(command.CustomerId);

        // 2. Add item lines (inventory/stock invariants are validated inside the Domain entity)
        foreach (OrderItemDto item in command.Items)
        {
            order.AddItem(
                item.ProductId,
                item.ProductName,
                item.Quantity,
                item.UnitPrice,
                item.AvailableStock
            );
        }

        // 3. Confirm order status transitions
        order.Confirm();

        // 4. Persist through the driven port
        await _orderRepository.SaveAsync(order, cancellationToken);

        // 5. Map and return response DTO
        return new OrderResponseDto(
            order.Id,
            order.CustomerId,
            order.Status,
            order.Total,
            order.CreatedAt
        );
    }
}