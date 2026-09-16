namespace Tienda.Infrastructure.Persistence.Entities;

/// <summary>
/// Database table mapping model representing an order item row.
/// </summary>
public class OrderItemEntity
{
    /// <summary>
    /// Gets or sets the primary key identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the foreign key pointing to the parent order entity.
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Gets or sets the associated product identifier.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ordered quantity.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Navigation property to the parent order entity.
    /// </summary>
    public OrderEntity Order { get; set; } = null!;
}