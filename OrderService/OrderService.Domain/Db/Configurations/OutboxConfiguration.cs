using Common.Domain.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Enums;
using OrderService.Domain.Models;

namespace OrderService.Domain.Db.Configurations;

public class OutboxConfiguration : ConfigurationBase<Outbox>
{
    public override void Configure(EntityTypeBuilder<Outbox> builder)
    {
        base.Configure(builder);

        builder.ToTable("outbox");

        builder.Property(o => o.Event).HasConversion(e => e.ToString(), s => Enum.Parse<OutboxEvent>(s)).HasColumnName("event").IsRequired();
        builder.Property(o => o.EntityId).HasColumnName("entity_id").IsRequired();
    }
}
