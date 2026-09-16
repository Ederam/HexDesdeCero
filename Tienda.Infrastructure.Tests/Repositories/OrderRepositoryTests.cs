using Microsoft.EntityFrameworkCore;
using Tienda.Domain.Entities;
using Tienda.Infrastructure.Persistence;
using Tienda.Infrastructure.Repositories;
using Xunit;

namespace Tienda.Infrastructure.Tests.Repositories;

/// <summary>
/// Integration tests suite for <see cref="OrderRepository"/> using Entity Framework Core InMemory database.
/// </summary>
public class OrderRepositoryTests
{
    private readonly DbContextOptions<ApplicationDbContext> _dbOptions;

    /// <summary>
    /// Initializes test setup by configuring an isolated InMemory database per test execution.
    /// </summary>
    public OrderRepositoryTests()
    {
        _dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task SaveAsync_WhenOrderIsValid_ShouldPersistOrderAndItemsInDatabase()
    {
        // ARRANGE
        using ApplicationDbContext context = new ApplicationDbContext(_dbOptions);
        OrderRepository repository = new OrderRepository(context);

        Order order = new Order("CLI-999");
        order.AddItem(Guid.NewGuid(), "Gaming Mouse", 2, 75000m, availableStock: 10);
        order.Confirm();

        // ACT
        await repository.SaveAsync(order);

        // ASSERT
        Order? persistedOrder = await repository.GetByIdAsync(order.Id);

        Assert.NotNull(persistedOrder);
        Assert.Equal("CLI-999", persistedOrder.CustomerId);
        Assert.Equal("Confirmed", persistedOrder.Status);
        Assert.Single(persistedOrder.Items);
        Assert.Equal(150000m, persistedOrder.Total);
    }
}