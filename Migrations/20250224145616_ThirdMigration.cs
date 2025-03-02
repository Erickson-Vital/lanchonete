using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lanchonete.Migrations
{
    /// <inheritdoc />
    public partial class ThirdMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LancheIngrediente",
                columns: table => new
                {
                    IngredientesId = table.Column<int>(type: "int", nullable: false),
                    LanchesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LancheIngrediente", x => new { x.IngredientesId, x.LanchesId });
                    table.ForeignKey(
                        name: "FK_LancheIngrediente_Ingredientes_IngredientesId",
                        column: x => x.IngredientesId,
                        principalTable: "Ingredientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LancheIngrediente_Lanches_LanchesId",
                        column: x => x.LanchesId,
                        principalTable: "Lanches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LancheIngrediente_LanchesId",
                table: "LancheIngrediente",
                column: "LanchesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LancheIngrediente");
        }
    }
}
