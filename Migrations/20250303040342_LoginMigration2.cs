using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace lanchonete.Migrations
{
    /// <inheritdoc />
    public partial class LoginMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1a7c2a16-61a8-4aaf-9d81-68302e1868e8", null, "admin", "cliente" },
                    { "78e53f68-fe64-4249-b9ab-750a0e269657", null, "cliente", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a7c2a16-61a8-4aaf-9d81-68302e1868e8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78e53f68-fe64-4249-b9ab-750a0e269657");
        }
    }
}
