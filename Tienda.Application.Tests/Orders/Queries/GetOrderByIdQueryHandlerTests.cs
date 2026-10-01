using Moq;
using Tienda.Application.Common.Models;
using Tienda.Application.Orders.Queries.GetOrderById;
using Tienda.Application.Orders.Queries.GetOrders;
using Tienda.Domain.Entities;
using Tienda.Domain.Ports;
using Xunit;

namespace Tienda.Application.Tests.Orders.Queries;

/// <summary>
/// Suite de pruebas unitarias para validar <see cref="GetOrdersQueryHandler"/>.
/// </summary>
public class GetOrdersQueryHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly GetOrdersQueryHandler _handler;

    /// <summary>
    /// Inicializa las dependencias simuladas y la instancia del Handler bajo prueba.
    /// </summary>
    public GetOrdersQueryHandlerTests()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _handler = new GetOrdersQueryHandler(_orderRepositoryMock.Object);
    }

    /// <summary>
    /// Verifica que se lance una excepción <see cref="ArgumentNullException"/> si el repositorio inyectado es nulo.
    /// </summary>
    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenRepositoryIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new GetOrdersQueryHandler(null!));
    }

    /// <summary>
    /// Verifica que la consulta paginada retorne el contrato esperado cuando existen registros.
    /// </summary>
    [Fact]
    public async Task Handle_ShouldReturnPagedResult_WhenOrdersExist()
    {
        // Arrange
        GetOrdersQuery query = new GetOrdersQuery(PageNumber: 1, PageSize: 10);
        List<Order> emptyOrdersList = new List<Order>();
        int expectedTotalCount = 0;

        _orderRepositoryMock
            .Setup(repo => repo.GetPagedAsync(query.PageNumber, query.PageSize, It.IsAny<CancellationToken>()))
            .ReturnsAsync((emptyOrdersList, expectedTotalCount));

        // Act
        PagedResult<OrderResponse> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(query.PageNumber, result.PageNumber);
        Assert.Equal(query.PageSize, result.PageSize);
        Assert.Equal(expectedTotalCount, result.TotalCount);

        _orderRepositoryMock.Verify(
            repo => repo.GetPagedAsync(query.PageNumber, query.PageSize, It.IsAny<CancellationToken>()),
            Times.Once
        );
    }
}