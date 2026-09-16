namespace Tienda.Infrastructure.Persistence.Entities;

/// <summary>
/// Database table mapping model representing an order header row.
/// </summary>
public class OrderEntity
{
    /// <summary>
    /// Gets or sets the primary key identifier of the order.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the customer identifier.
    /// </summary>
    public string CustomerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the UTC creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the current status string.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the relational collection of order items.
    /// </summary>
    public List<OrderItemEntity> Items { get; set; } = new List<OrderItemEntity>();
}