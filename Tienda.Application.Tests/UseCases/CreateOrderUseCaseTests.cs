using NSubstitute;
using Tienda.Application.Dtos;
using Tienda.Application.Orders.CreateOrder;
using Tienda.Domain.Entities;
using Tienda.Domain.Ports;
using Xunit;

namespace Tienda.Application.Tests.Orders;

public class CreateOrderCommandHandlerTests
{
    private readonly IOrderRepository _orderRepositoryMock;
    private readonly CreateOrderCommandHandler _handler;

    public CreateOrderCommandHandlerTests()
    {
        _orderRepositoryMock = Substitute.For<IOrderRepository>();
        _handler = new CreateOrderCommandHandler(_orderRepositoryMock);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldSaveOrderAndReturnResponseDto()
    {
        // Arrange
        var command = new CreateOrderCommand(
            "CLI-123",
            new List<CreateOrderItemCommand>
            {
                new(Guid.NewGuid(), "Teclado", 2, 50000m, 10)
            }
        );

        // Act
        OrderResponseDto response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal("CLI-123", response.CustomerId);
        Assert.Equal(100000m, response.Total);
        Assert.Single(response.Items);

        await _orderRepositoryMock.Received(1).SaveAsync(Arg.Any<Order>());
    }
}