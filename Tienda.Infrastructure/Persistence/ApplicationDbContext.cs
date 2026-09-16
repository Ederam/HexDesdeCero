using Microsoft.EntityFrameworkCore;
using Tienda.Infrastructure.Persistence.Entities;

namespace Tienda.Infrastructure.Persistence;

/// <summary>
/// Entity Framework Core database context handling persistence operations for the application.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">The context configuration options.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the orders database set.
    /// </summary>
    public DbSet<OrderEntity> Orders => Set<OrderEntity>();

    /// <summary>
    /// Gets or sets the order items database set.
    /// </summary>
    public DbSet<OrderItemEntity> OrderItems => Set<OrderItemEntity>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrderEntity>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.CustomerId).IsRequired().HasMaxLength(50);
            entity.Property(o => o.Status).IsRequired().HasMaxLength(20);
            entity.HasMany(o => o.Items)
                  .WithOne(i => i.Order)
                  .HasForeignKey(i => i.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItemEntity>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.ProductName).IsRequired().HasMaxLength(100);
            entity.Property(i => i.UnitPrice).HasPrecision(18, 2);
        });
    }
}