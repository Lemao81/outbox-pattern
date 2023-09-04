using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Models;

namespace OrderService.Domain.Db.Configurations;

public class OrderConfiguration : ConfigurationBase<Order>
{
    public override void Configure(EntityTypeBuilder<Order> builder)
    {
        base.Configure(builder);

        builder.ToTable("orders");

        builder.Property(o => o.Status).HasColumnName("status").IsRequired();
        builder.Property(o => o.TotalAmount).HasColumnName("total_amount");

        builder.HasMany(o => o.Products).WithMany(p => p.Orders).UsingEntity(b =>
        {
            b.ToTable("order_product");

            b.Property(typeof(Guid), "OrdersId").HasColumnName("orders_id");
            b.Property(typeof(Guid), "ProductsId").HasColumnName("products_id");
        });
    }
}
