using OrderService.Domain.Enums;
using OrderService.Domain.Models;

namespace OrderService.Domain.Db.Seeds;

public static class ProductSeed
{
    public static IEnumerable<Product> GetSeededProducts() =>
        new[]
        {
            new Product
            {
                Id = Guid.Parse("ac64573c-10f7-4e9a-ab52-9a79c148c262"),
                Category = ProductCategory.Clothing,
                Name = "Denver Jeans",
                Price = 74.95M
            },
            new Product
            {
                Id = Guid.Parse("15022ede-5c00-4348-b158-01f931bcd506"),
                Category = ProductCategory.Clothing,
                Name = "Bershka Collar",
                Price = 22.99M
            },
            new Product
            {
                Id = Guid.Parse("a674ff3e-4a5b-4d5b-87e3-6fad2e214140"),
                Category = ProductCategory.Electronics,
                Name = "Raspberry",
                Price = 125.99M
            },
            new Product
            {
                Id = Guid.Parse("9e418a9e-d2fe-497f-8a31-3dc78054a2f0"),
                Category = ProductCategory.Electronics,
                Name = "Headset",
                Price = 31.99M
            },
            new Product
            {
                Id = Guid.Parse("f152c8b2-b79e-4ee6-8079-06269a7154e5"),
                Category = ProductCategory.Sports,
                Name = "Football",
                Price = 126.99M
            }
        };
}
