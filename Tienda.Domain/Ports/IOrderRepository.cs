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

    /// <summary>
    /// Retrieves a paged collection of orders asynchronously.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of orders to retrieve per page.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with the collection of
    /// orders and the total number of orders.</returns>
    Task<(IReadOnlyCollection<Order> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}