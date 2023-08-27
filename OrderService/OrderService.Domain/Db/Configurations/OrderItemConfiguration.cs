using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Models;

namespace OrderService.Domain.Db.Configurations;

public class OrderItemConfiguration : ConfigurationBase<OrderItem>
{
    public override void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        base.Configure(builder);

        builder.ToTable("order_items");

        builder.Property(i => i.OrderId).HasColumnName("order_id");
    }
}
