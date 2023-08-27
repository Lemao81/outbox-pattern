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

        builder.HasMany(o => o.Items).WithOne().HasForeignKey(i => i.OrderId);
    }
}
