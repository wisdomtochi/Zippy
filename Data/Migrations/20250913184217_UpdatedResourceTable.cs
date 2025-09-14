using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zippy.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedResourceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "resources",
                keyColumn: "Id",
                keyValue: new Guid("dfced651-e327-4052-8bc6-7c739c86f431"));

            migrationBuilder.DropColumn(
                name: "Name",
                table: "resources");

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "resources",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "resources",
                columns: new[] { "Id", "Alias", "CreatedAt", "Key", "Url" },
                values: new object[] { new Guid("b0c3a065-4f8b-4fcd-b924-6b317417e18c"), "Resource 1", new DateTime(2025, 9, 13, 18, 42, 15, 947, DateTimeKind.Utc).AddTicks(3200), "resource1-key", "https://github.com/wisdomtochi" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "resources",
                keyColumn: "Id",
                keyValue: new Guid("b0c3a065-4f8b-4fcd-b924-6b317417e18c"));

            migrationBuilder.DropColumn(
                name: "Alias",
                table: "resources");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "resources",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "resources",
                columns: new[] { "Id", "CreatedAt", "Key", "Name", "Url" },
                values: new object[] { new Guid("dfced651-e327-4052-8bc6-7c739c86f431"), new DateTime(2025, 9, 12, 1, 39, 28, 456, DateTimeKind.Utc).AddTicks(7538), "resource1-key", "Resource 1", "https://github.com/wisdomtochi" });
        }
    }
}
