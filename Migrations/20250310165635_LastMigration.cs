using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lanchonete.Migrations
{
    /// <inheritdoc />
    public partial class LastMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Inserir ingredientes
            migrationBuilder.InsertData(
                table: "Ingredientes",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Imposta da Shopee", 10.00m },
                    { 2, "Imposto sobre o Impost", 5.00m },
                    { 3, "Faz o L", 15.00m }
                });

            // Inserir lanche com imagem
            migrationBuilder.InsertData(
                table: "Lanches",
                columns: new[] { "Id", "Name", "Price", "Image", "ImageMimiType" },
                values: new object[]
                {
                    1,
                    "X - L",
                    30.00m,
                    // Hex string convertida em bytes
                    Convert.FromHexString("FFD8FFE000104A46494600010100000100010000FFE201D84943435F50524F46494C45000101000001C800000000043000006D6E74725247422058595A2007E00001000100000000000061637370000000000000000000000000000000000000000000000000000000010000F6D6000100000000D32D00000000000000000000"),
                    "image/jpeg"
                });

            // Relacionar lanche com os ingredientes
            migrationBuilder.InsertData(
                table: "LancheIngrediente",
                columns: new[] { "IngredientesId", "LanchesId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("LancheIngrediente", "IngredientesId", 1);
            migrationBuilder.DeleteData("LancheIngrediente", "IngredientesId", 2);
            migrationBuilder.DeleteData("LancheIngrediente", "IngredientesId", 3);

            migrationBuilder.DeleteData("Lanches", "Id", 1);

            migrationBuilder.DeleteData("Ingredientes", "Id", 1);
            migrationBuilder.DeleteData("Ingredientes", "Id", 2);
            migrationBuilder.DeleteData("Ingredientes", "Id", 3);
        }
    }
}
