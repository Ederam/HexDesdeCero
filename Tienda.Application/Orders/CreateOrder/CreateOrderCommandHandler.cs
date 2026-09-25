using MediatR;
using Tienda.Application.Dtos;
using Tienda.Domain.Entities;
using Tienda.Domain.Ports;

namespace Tienda.Application.Orders.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderResponseDto>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
    }

    public async Task<OrderResponseDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        Order order = new Order(request.CustomerId);

        foreach (var item in request.Items)
        {
            order.AddItem(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice, item.AvailableStock);
        }

        order.Confirm();

        await _orderRepository.SaveAsync(order);

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