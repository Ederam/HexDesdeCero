using Tienda.Domain.Entities;
using Tienda.Infrastructure.Persistence.Entities;

namespace Tienda.Infrastructure.Mappers;

/// <summary>
/// Static mapping utility to translate between Domain Entities and Persistence Data Entities.
/// </summary>
public static class OrderMapper
{
    /// <summary>
    /// Maps a domain aggregate <see cref="Order"/> into a database entity <see cref="OrderEntity"/>.
    /// </summary>
    /// <param name="domainOrder">The domain order aggregate root.</param>
    /// <returns>The mapped database entity.</returns>
    public static OrderEntity ToEntity(Order domainOrder)
    {
        ArgumentNullException.ThrowIfNull(domainOrder);

        OrderEntity entity = new OrderEntity
        {
            Id = domainOrder.Id,
            CustomerId = domainOrder.CustomerId,
            CreatedAt = domainOrder.CreatedAt,
            Status = domainOrder.Status,
            Items = domainOrder.Items.Select(item => new OrderItemEntity
            {
                Id = Guid.NewGuid(),
                OrderId = domainOrder.Id,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList()
        };

        return entity;
    }
}