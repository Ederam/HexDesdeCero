using Microsoft.EntityFrameworkCore;
using Tienda.Domain.Entities;
using Tienda.Domain.Ports;
using Tienda.Infrastructure.Mappers;
using Tienda.Infrastructure.Persistence;
using Tienda.Infrastructure.Persistence.Entities;

namespace Tienda.Infrastructure.Repositories;

/// <summary>
/// Concrete implementation of <see cref="IOrderRepository"/> port using Entity Framework Core.
/// </summary>
public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderRepository"/> class.
    /// </summary>
    /// <param name="context">The database context instance.</param>
    public OrderRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc />
    public async Task SaveAsync(Order order, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(order);

        OrderEntity entity = OrderMapper.ToEntity(order);
        await _context.Orders.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        OrderEntity? entity = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        if (entity == null)
        {
            return null;
        }

        // Reconstruct aggregate root from data entity
        Order order = new Order(entity.CustomerId);
        foreach (OrderItemEntity item in entity.Items)
        {
            // Using int.MaxValue for stock on retrieval since persisted orders already satisfied stock validation
            order.AddItem(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice, availableStock: int.MaxValue);
        }

        return order;
    }

    /// <summary>
    /// Obtiene un conjunto paginado de órdenes.
    /// </summary>
    /// <param name="pageNumber">Número de página actual.</param>
    /// <param name="pageSize">Cantidad de registros por página.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Una tupla con la colección de órdenes de dominio y el conteo total de registros.</returns>
    public async Task<(IReadOnlyCollection<Order> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        int totalCount = await _context.Orders.CountAsync(cancellationToken);

        List<OrderEntity> entityList = await _context.Orders
            .Include(o => o.Items)
            .OrderByDescending(o => o.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        // Reconstrucción de la lista de agregados utilizando los comportamientos del dominio
        List<Order> domainOrders = entityList.Select((OrderEntity entity) => {
            Order order = new Order(entity.CustomerId);

            foreach (OrderItemEntity itemEntity in entity.Items)
            {
                order.AddItem(itemEntity.ProductId, itemEntity.ProductName, itemEntity.Quantity, itemEntity.UnitPrice, availableStock: int.MaxValue);
            }

            return order;
        }).ToList();

        return (domainOrders, totalCount);
    }
}