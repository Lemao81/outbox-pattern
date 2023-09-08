using System.Reflection;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Db.Seeds;
using OrderService.Domain.Models;

namespace OrderService.Domain.Db;

public class OrderServiceDbContext : DbContext
{
    public DbSet<Order>? Orders { get; set; }
    public DbSet<Product>? Products { get; set; }
    public DbSet<Outbox>? Outboxes { get; set; }

    public OrderServiceDbContext(DbContextOptions<OrderServiceDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Product>().HasData(ProductSeed.GetSeededProducts());
    }
}
