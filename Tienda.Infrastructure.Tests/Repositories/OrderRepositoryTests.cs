using Xunit;
using Microsoft.EntityFrameworkCore;
using Tienda.Domain.Entities;
using Tienda.Infrastructure.Persistence;
using Tienda.Infrastructure.Repositories;

namespace Tienda.Infrastructure.Tests.Repositories;

public class OrderRepositoryTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly OrderRepository _repository;

    public OrderRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _repository = new OrderRepository(_dbContext);
    }

    [Fact]
    public async Task SaveAsync_WhenOrderIsValid_ShouldPersistOrderAndItemsInDatabase()
    {
        // Arrange
        var order = new Order("CLI-123");
        order.AddItem(Guid.NewGuid(), "Teclado", 2, 50000m, 10);
        order.Confirm(); // Altera o status de Draft para Confirmed

        // Act
        await _repository.SaveAsync(order);
        var savedOrder = await _dbContext.Orders.FindAsync(order.Id);

        // Assert
        Assert.NotNull(savedOrder);
        Assert.Equal("Confirmed", savedOrder.Status.ToString());
    }
}