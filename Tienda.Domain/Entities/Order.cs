namespace Tienda.Domain.Entities;

/// <summary>
/// Aggregate root representing a customer order and encapsulating its business invariant rules.
/// </summary>
public class Order
{
    private readonly List<OrderItem> _items = new();

    public Guid Id { get; }
    public string CustomerId { get; }
    public DateTime CreatedAt { get; }
    public string Status { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public decimal Total => _items.Sum(item => item.SubTotal);

    /// <summary>
    /// Initializes a new instance of the <see cref="Order"/> class in 'Draft' status (Nacimiento).
    /// </summary>
    public Order(string customerId)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        CreatedAt = DateTime.UtcNow;
        Status = "Draft";
    }

    /// <summary>
    /// Constructor privado para reconstituir el objeto desde la base de datos.
    /// </summary>
    private Order(Guid id, string customerId, DateTime createdAt, string status)
    {
        Id = id;
        CustomerId = customerId;
        CreatedAt = createdAt;
        Status = status;
    }

    public void AddItem(Guid productId, string productName, int quantity, decimal unitPrice, int availableStock)
    {
        OrderItem item = new OrderItem(productId, productName, quantity, unitPrice, availableStock);
        _items.Add(item);
    }

    public void Confirm()
    {
        if (!_items.Any())
        {
            throw new InvalidOperationException("Cannot confirm an empty order.");
        }

        Status = "Confirmed";
    }

    // --- MÉTODOS DE HIDRATACIÓN (Para Infraestructura) ---

    /// <summary>
    /// Factory Method estático para reconstruir la orden desde la base de datos conservando su ID y Fecha originales.
    /// </summary>
    public static Order Hydrate(Guid id, string customerId, DateTime createdAt, string status)
    {
        return new Order(id, customerId, createdAt, status);
    }

    /// <summary>
    /// Método para cargar ítems históricos sin disparar validaciones de stock.
    /// </summary>
    public void LoadExistingItem(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        OrderItem item = OrderItem.Hydrate(productId, productName, quantity, unitPrice);
        _items.Add(item);
    }
}