using Moq;
using Tienda.Application.Orders.Commands.CreateOrder;
using Tienda.Domain.Entities;
using Tienda.Domain.Ports;
using Xunit;

namespace Tienda.Application.Tests.Orders.Commands;

public class CreateOrderCommandHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly CreateOrderCommandHandler _handler;

    public CreateOrderCommandHandlerTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _handler = new CreateOrderCommandHandler(_orderRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateAndSaveOrder_WhenCommandIsValid()
    {
        Guid productId = Guid.NewGuid();
        CreateOrderCommand command = new CreateOrderCommand(
            CustomerName: "Camilo Ramírez",
            Items: new List<CreateOrderItemCommand>
            {
                new CreateOrderItemCommand(productId, Quantity: 2, UnitPrice: 100.0m)
            }
        );

        CreateOrderResponse response = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(response);
        Assert.NotEqual(Guid.Empty, response.Id);
        Assert.Equal("Camilo Ramírez", response.CustomerName);
        Assert.Equal(200.0m, response.TotalAmount);

        _orderRepositoryMock.Verify(
            repo => repo.SaveAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}