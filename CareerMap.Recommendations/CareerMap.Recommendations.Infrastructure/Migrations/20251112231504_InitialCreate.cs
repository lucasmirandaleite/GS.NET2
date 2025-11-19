using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CareerMap.Recommendations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carreiras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Area = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Nivel = table.Column<int>(type: "INTEGER", nullable: false),
                    CompetenciasNecessarias = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carreiras", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Competencias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competencias", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Carreiras",
                columns: new[] { "Id", "Area", "CompetenciasNecessarias", "Descricao", "Nivel", "Nome" },
                values: new object[] { 1, "Tecnologia", "[{\"Id\":1,\"Nome\":\"C#\",\"Tipo\":\"Hard Skill\"},{\"Id\":2,\"Nome\":\".NET Core\",\"Tipo\":\"Hard Skill\"},{\"Id\":3,\"Nome\":\"SQL\",\"Tipo\":\"Hard Skill\"}]", "Foco em APIs RESTful com .NET e Entity Framework.", 1, "Desenvolvedor Backend .NET Júnior" });

            migrationBuilder.InsertData(
                table: "Competencias",
                columns: new[] { "Id", "Nome", "Tipo" },
                values: new object[,]
                {
                    { 1, "C#", "Hard Skill" },
                    { 2, ".NET Core", "Hard Skill" },
                    { 3, "SQL", "Hard Skill" },
                    { 4, "Comunicação", "Soft Skill" },
                    { 5, "Resolução de Problemas", "Soft Skill" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Carreiras");

            migrationBuilder.DropTable(
                name: "Competencias");
        }
    }
}
