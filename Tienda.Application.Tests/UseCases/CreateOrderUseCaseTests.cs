using NSubstitute;
using Tienda.Application.Dtos;
using Tienda.Application.UseCases;
using Tienda.Domain.Entities;
using Tienda.Domain.Exceptions;
using Tienda.Domain.Ports;
using Xunit;

namespace Tienda.Application.Tests.UseCases;

/// <summary>
/// Unit tests suite for verifying orchestration and interaction logic in <see cref="CreateOrderUseCase"/>.
/// </summary>
public class CreateOrderUseCaseTests
{
    private readonly IOrderRepository _orderRepositoryMock;
    private readonly CreateOrderUseCase _useCase;

    /// <summary>
    /// Initializes test setup by creating repository mocks and instantiating the SUT.
    /// </summary>
    public CreateOrderUseCaseTests()
    {
        // ARRANGE GLOBAL: Mock the IOrderRepository port using NSubstitute
        _orderRepositoryMock = Substitute.For<IOrderRepository>();

        // Instantiate System Under Test (SUT)
        _useCase = new CreateOrderUseCase(_orderRepositoryMock);
    }

    [Fact]
    public async Task ExecuteAsync_WhenOrderIsValid_ShouldSaveOrderAndReturnResponseDto()
    {
        // ARRANGE
        var command = new CreateOrderDto(
            CustomerId: "CLI-001",
            Items: new List<OrderItemDto>
            {
                new(Guid.NewGuid(), "Mechanical Keyboard", 1, 150000m, AvailableStock: 10)
            }
        );

        // ACT
        var result = await _useCase.ExecuteAsync(command);

        // ASSERT
        Assert.NotNull(result);
        Assert.Equal("CLI-001", result.CustomerId);
        Assert.Equal("Confirmed", result.Status);
        Assert.Equal(150000m, result.Total);

        // Verify interaction: Ensure SaveAsync was called exactly once on the port
        await _orderRepositoryMock.Received(1).SaveAsync(Arg.Any<Order>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteAsync_WhenStockIsInsufficient_ShouldThrowInsufficientStockException()
    {
        // ARRANGE: Requested quantity (5) exceeds available stock (2)
        var command = new CreateOrderDto(
            CustomerId: "CLI-001",
            Items: new List<OrderItemDto>
            {
                new(Guid.NewGuid(), "Mechanical Keyboard", 5, 150000m, AvailableStock: 2)
            }
        );

        // ACT & ASSERT: Verify that domain exception is thrown
        await Assert.ThrowsAsync<InsufficientStockException>(() => _useCase.ExecuteAsync(command));

        // Verify interaction: Confirm SaveAsync was NEVER called when validation failed
        await _orderRepositoryMock.DidNotReceive().SaveAsync(Arg.Any<Order>(), Arg.Any<CancellationToken>());
    }
}