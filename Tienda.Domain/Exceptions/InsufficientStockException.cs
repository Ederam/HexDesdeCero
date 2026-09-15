namespace Tienda.Domain.Exceptions;

/// <summary>
/// Domain exception thrown when the requested item quantity exceeds the available stock.
/// </summary>
public class InsufficientStockException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InsufficientStockException"/> class.
    /// </summary>
    /// <param name="productName">The name of the product with insufficient stock.</param>
    /// <param name="requestedQuantity">The quantity requested by the user.</param>
    /// <param name="availableStock">The current available stock in inventory.</param>
    public InsufficientStockException(string productName, int requestedQuantity, int availableStock)
        : base($"Insufficient stock for '{productName}'. Requested: {requestedQuantity}, Available: {availableStock}.")
    {
    }
}