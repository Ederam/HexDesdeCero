using Tienda.Domain.Exceptions;

namespace Tienda.Domain.Entities;

/// <summary>
/// Represents an individual item within an order line.
/// </summary>
public class OrderItem
{
    /// <summary>
    /// Gets the unique identifier of the product.
    /// </summary>
    public Guid ProductId { get; }

    /// <summary>
    /// Gets the name of the product.
    /// </summary>
    public string ProductName { get; }

    /// <summary>
    /// Gets the ordered quantity of the product.
    /// </summary>
    public int Quantity { get; }

    /// <summary>
    /// Gets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; }

    /// <summary>
    /// Gets the calculated subtotal for this item line.
    /// </summary>
    public decimal SubTotal => Quantity * UnitPrice;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderItem"/> class and validates stock invariants.
    /// </summary>
    /// <param name="productId">The unique product identifier.</param>
    /// <param name="productName">The product name.</param>
    /// <param name="quantity">The requested quantity.</param>
    /// <param name="unitPrice">The unit price.</param>
    /// <param name="availableStock">The available stock to validate against invariant rules.</param>
    /// <exception cref="InsufficientStockException">Thrown when quantity exceeds available stock.</exception>
    public OrderItem(Guid productId, string productName, int quantity, decimal unitPrice, int availableStock)
    {
        if (quantity > availableStock)
        {
            throw new InsufficientStockException(productName, quantity, availableStock);
        }

        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}