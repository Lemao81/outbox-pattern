using Common.Domain.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Enums;
using OrderService.Domain.Models;

namespace OrderService.Domain.Db.Configurations;

public class ProductConfiguration : ConfigurationBase<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder.ToTable("products");

        builder.Property(p => p.Category).HasConversion(c => c.ToString(), s => Enum.Parse<ProductCategory>(s)).HasColumnName("category").IsRequired();
        builder.Property(p => p.Name).HasColumnName("name").IsRequired();
        builder.Property(p => p.Price).HasColumnName("price");
    }
}
