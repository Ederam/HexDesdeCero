namespace Tienda.Domain.Entities;

public class OrderItem
{
    public Guid ProductId { get; }
    public string ProductName { get; }
    public int Quantity { get; }
    public decimal UnitPrice { get; }
    public decimal SubTotal => Quantity * UnitPrice;

    /// <summary>
    /// Constructor principal para creación de nuevos ítems (Aplica validaciones de negocio).
    /// </summary>
    public OrderItem(Guid productId, string productName, int quantity, decimal unitPrice, int availableStock)
    {
        if (quantity <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero.");
        if (quantity > availableStock) throw new InvalidOperationException("No hay stock suficiente.");

        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    /// <summary>
    /// Constructor privado para reconstitución (Sin validación de stock, pues ya ocurrió en el pasado).
    /// </summary>
    private OrderItem(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    /// <summary>
    /// Factory Method para uso exclusivo de la capa de Infraestructura.
    /// </summary>
    public static OrderItem Hydrate(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        return new OrderItem(productId, productName, quantity, unitPrice);
    }
}