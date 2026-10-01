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

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task SaveAsync(Order order, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(order);

        OrderEntity entity = OrderMapper.ToEntity(order);
        await _context.Orders.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        OrderEntity? entity = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        if (entity == null)
        {
            return null;
        }

        // Hidratación del Aggregate Root conservando Id y CreatedAt originales
        Order order = Order.Hydrate(entity.Id, entity.CustomerId, entity.CreatedAt, "Draft");

        foreach (OrderItemEntity itemEntity in entity.Items)
        {
            // Carga histórica sin validaciones de negocio
            order.LoadExistingItem(itemEntity.ProductId, itemEntity.ProductName, itemEntity.Quantity, itemEntity.UnitPrice);
        }

        return order;
    }

    public async Task<(IReadOnlyCollection<Order> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        int totalCount = await _context.Orders.CountAsync(cancellationToken);

        List<OrderEntity> entityList = await _context.Orders
            .Include(o => o.Items)
            .OrderByDescending(o => o.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        List<Order> domainOrders = entityList.Select((OrderEntity entity) => {
            Order order = Order.Hydrate(entity.Id, entity.CustomerId, entity.CreatedAt, "Draft");

            foreach (OrderItemEntity itemEntity in entity.Items)
            {
                order.LoadExistingItem(itemEntity.ProductId, itemEntity.ProductName, itemEntity.Quantity, itemEntity.UnitPrice);
            }

            return order;
        }).ToList();

        return (domainOrders, totalCount);
    }
}