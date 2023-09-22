using Common.Domain.Db;
using DeliveryService.Domain.Enums;
using DeliveryService.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryService.Domain.Db.Configurations;

public class DeliveryConfiguration : ConfigurationBase<Delivery>
{
    public override void Configure(EntityTypeBuilder<Delivery> builder)
    {
        base.Configure(builder);

        builder.ToTable("deliveries");

        builder.Property(d => d.Status).HasConversion(s => s.ToString(), s => Enum.Parse<DeliveryStatus>(s)).HasColumnName("status").IsRequired();
        builder.Property(d => d.OrderId).HasColumnName("order_id").IsRequired();
        builder.Property(d => d.Name).HasColumnName("name").IsRequired();
        builder.Property(d => d.Address).HasColumnName("address").IsRequired();

        builder.HasIndex(d => d.OrderId).IsUnique();
    }
}
