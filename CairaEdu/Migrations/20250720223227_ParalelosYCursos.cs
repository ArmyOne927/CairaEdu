using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CairaEdu.Migrations
{
    /// <inheritdoc />
    public partial class ParalelosYCursos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Curso",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    curso_nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    curso_ciclo_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Curso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Curso_CicloLectivo",
                        column: x => x.curso_ciclo_id,
                        principalTable: "CiclosLectivos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Paralelo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    paral_nombre = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    paral_curso_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paralelo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paralelo_Curso",
                        column: x => x.paral_curso_id,
                        principalTable: "Curso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Curso_curso_ciclo_id",
                table: "Curso",
                column: "curso_ciclo_id");

            migrationBuilder.CreateIndex(
                name: "UX_Curso_Nombre_Ciclo",
                table: "Curso",
                columns: new[] { "curso_nombre", "curso_ciclo_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Paralelo_paral_curso_id",
                table: "Paralelo",
                column: "paral_curso_id");

            migrationBuilder.CreateIndex(
                name: "UX_Paralelo_Nombre_Curso",
                table: "Paralelo",
                columns: new[] { "paral_nombre", "paral_curso_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Paralelo");

            migrationBuilder.DropTable(
                name: "Curso");
        }
    }
}
