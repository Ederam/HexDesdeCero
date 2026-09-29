using NSubstitute;
using Tienda.Application.Dtos;
using Tienda.Application.Orders.GetOrderById;
using Tienda.Domain.Entities;
using Tienda.Domain.Exceptions;
using Tienda.Domain.Ports;
using Xunit;

namespace Tienda.Application.Tests.Orders;

public class GetOrderByIdQueryHandlerTests
{
    private readonly IOrderRepository _orderRepositoryMock;
    private readonly GetOrderByIdQueryHandler _handler;

    public GetOrderByIdQueryHandlerTests()
    {
        _orderRepositoryMock = Substitute.For<IOrderRepository>();
        _handler = new GetOrderByIdQueryHandler(_orderRepositoryMock);
    }

    [Fact]
    public async Task Handle_WhenOrderExists_ShouldReturnOrderResponseDto()
    {
        // Arrange
        Guid orderId = Guid.NewGuid();
        Order existingOrder = new Order("CLI-001");
        existingOrder.AddItem(Guid.NewGuid(), "Mouse", 1, 30000m, 5);

        // Forzar asignación de Id para la prueba simulada mediante reflexión o entidad limpia
        _orderRepositoryMock.GetByIdAsync(orderId).Returns(Task.FromResult<Order?>(existingOrder));

        // Act
        OrderResponseDto response = await _handler.Handle(new GetOrderByIdQuery(orderId), CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("CLI-001", response.CustomerId);
        Assert.Equal(30000m, response.Total);
    }

    [Fact]
    public async Task Handle_WhenOrderDoesNotExist_ShouldThrowOrderNotFoundException()
    {
        // Arrange
        Guid nonExistentOrderId = Guid.NewGuid();
        _orderRepositoryMock.GetByIdAsync(nonExistentOrderId).Returns(Task.FromResult<Order?>(null));

        // Act & Assert
        await Assert.ThrowsAsync<OrderNotFoundException>(() =>
            _handler.Handle(new GetOrderByIdQuery(nonExistentOrderId), CancellationToken.None));
    }
}