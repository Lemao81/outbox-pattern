using System.Reflection;
using DeliveryService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Domain.Db;

public class DeliveryServiceDbContext : DbContext
{
    public DbSet<Delivery>? Deliveries { get; set; }

    public DeliveryServiceDbContext(DbContextOptions<DeliveryServiceDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
