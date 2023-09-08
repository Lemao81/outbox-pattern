using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OrderService.Domain.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddProductSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "id", "category", "created_at", "modified_at", "name", "price" },
                values: new object[,]
                {
                    { new Guid("15022ede-5c00-4348-b158-01f931bcd506"), "Clothing", new DateTime(2023, 9, 8, 18, 58, 25, 433, DateTimeKind.Utc).AddTicks(4467), new DateTime(2023, 9, 8, 18, 58, 25, 433, DateTimeKind.Utc).AddTicks(4467), "Bershka Collar", 22.99m },
                    { new Guid("9e418a9e-d2fe-497f-8a31-3dc78054a2f0"), "Electronics", new DateTime(2023, 9, 8, 18, 58, 25, 433, DateTimeKind.Utc).AddTicks(4471), new DateTime(2023, 9, 8, 18, 58, 25, 433, DateTimeKind.Utc).AddTicks(4471), "Headset", 31.99m },
                    { new Guid("a674ff3e-4a5b-4d5b-87e3-6fad2e214140"), "Electronics", new DateTime(2023, 9, 8, 18, 58, 25, 433, DateTimeKind.Utc).AddTicks(4469), new DateTime(2023, 9, 8, 18, 58, 25, 433, DateTimeKind.Utc).AddTicks(4469), "Raspberry", 125.99m },
                    { new Guid("ac64573c-10f7-4e9a-ab52-9a79c148c262"), "Clothing", new DateTime(2023, 9, 8, 18, 58, 25, 433, DateTimeKind.Utc).AddTicks(4449), new DateTime(2023, 9, 8, 18, 58, 25, 433, DateTimeKind.Utc).AddTicks(4451), "Denver Jeans", 74.95m },
                    { new Guid("f152c8b2-b79e-4ee6-8079-06269a7154e5"), "Sports", new DateTime(2023, 9, 8, 18, 58, 25, 433, DateTimeKind.Utc).AddTicks(4472), new DateTime(2023, 9, 8, 18, 58, 25, 433, DateTimeKind.Utc).AddTicks(4472), "Football", 126.99m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: new Guid("15022ede-5c00-4348-b158-01f931bcd506"));

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: new Guid("9e418a9e-d2fe-497f-8a31-3dc78054a2f0"));

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: new Guid("a674ff3e-4a5b-4d5b-87e3-6fad2e214140"));

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: new Guid("ac64573c-10f7-4e9a-ab52-9a79c148c262"));

            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValue: new Guid("f152c8b2-b79e-4ee6-8079-06269a7154e5"));
        }
    }
}
