namespace Tienda.Domain.Entities;

/// <summary>
/// Aggregate root representing a customer order and encapsulating its business invariant rules.
/// </summary>
public class Order
{
    private readonly List<OrderItem> _items = new();

    /// <summary>
    /// Gets the unique identifier of the order.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the customer identifier associated with this order.
    /// </summary>
    public string CustomerId { get; }

    /// <summary>
    /// Gets the UTC creation date and time of the order.
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Gets the current status of the order (e.g., "Draft", "Confirmed").
    /// </summary>
    public string Status { get; private set; }

    /// <summary>
    /// Gets a read-only collection of order items, preserving encapsulation.
    /// </summary>
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    /// <summary>
    /// Gets the total calculated amount of the order.
    /// </summary>
    public decimal Total => _items.Sum(item => item.SubTotal);

    /// <summary>
    /// Initializes a new instance of the <see cref="Order"/> class in 'Draft' status.
    /// </summary>
    /// <param name="customerId">The unique customer identifier.</param>
    public Order(string customerId)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        CreatedAt = DateTime.UtcNow;
        Status = "Draft";
    }

    /// <summary>
    /// Adds an item line to the order validating inventory constraints.
    /// </summary>
    /// <param name="productId">The product identifier.</param>
    /// <param name="productName">The product name.</param>
    /// <param name="quantity">The requested quantity.</param>
    /// <param name="unitPrice">The unit price.</param>
    /// <param name="availableStock">The available stock in inventory.</param>
    public void AddItem(Guid productId, string productName, int quantity, decimal unitPrice, int availableStock)
    {
        var item = new OrderItem(productId, productName, quantity, unitPrice, availableStock);
        _items.Add(item);
    }

    /// <summary>
    /// Confirms the order status if invariant rules are met.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when attempting to confirm an empty order.</exception>
    public void Confirm()
    {
        if (!_items.Any())
        {
            throw new InvalidOperationException("Cannot confirm an empty order.");
        }

        Status = "Confirmed";
    }
}