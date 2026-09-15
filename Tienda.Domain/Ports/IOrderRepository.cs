using Tienda.Domain.Entities;

namespace Tienda.Domain.Ports;

/// <summary>
/// Port contract defining persistence operations for the <see cref="Order"/> aggregate.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Persists an order instance asynchronously.
    /// </summary>
    /// <param name="order">The order aggregate root to save.</param>
    /// <param name="cancellationToken">Cancellation token signal.</param>
    Task SaveAsync(Order order, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an order aggregate by its unique identifier.
    /// </summary>
    /// <param name="id">The unique order identifier.</param>
    /// <param name="cancellationToken">Cancellation token signal.</param>
    /// <returns>The order aggregate if found; otherwise, null.</returns>
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}