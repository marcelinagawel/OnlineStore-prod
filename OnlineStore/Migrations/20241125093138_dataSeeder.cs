using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStore.Migrations
{
    /// <inheritdoc />
    public partial class dataSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Dodanie danych testowych do tabeli Categories
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
            { 1, "Elektronika" },
            { 2, "Książki" },
            { 3, "Odzież" }
                });

            // Dodanie danych testowych do tabeli Products
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Price", "Quantity", "Image", "CategoryId" },
                values: new object[,]
                {
            { 1, "Smartphone", 2999.99m, 10, "smartphone.jpg", 1 },
            { 2, "Książka ASP.NET Core", 59.99m, 50, "book.jpg", 2 },
            { 3, "Koszulka", 19.99m, 100, "tshirt.jpg", 3 }
                });

            // Dodanie danych testowych do tabeli Users
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name", "Email", "Password", "IsAdmin" },
                values: new object[,]
                {
            { 1, "Admin", "admin@example.com", "123456", true },
            { 2, "User", "user@example.com", "password", false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
                    migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3 });

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3 });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2 });
        }
    }
}
