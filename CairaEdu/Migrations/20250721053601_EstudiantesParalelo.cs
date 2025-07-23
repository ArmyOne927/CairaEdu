using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CairaEdu.Migrations
{
    /// <inheritdoc />
    public partial class EstudiantesParalelo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstudiantesXParalelo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstudianteId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ParaleloId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstudiantesXParalelo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstudiantesXParalelo_AspNetUsers_EstudianteId",
                        column: x => x.EstudianteId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstudiantesXParalelo_Paralelo_ParaleloId",
                        column: x => x.ParaleloId,
                        principalTable: "Paralelo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstudiantesXParalelo_EstudianteId",
                table: "EstudiantesXParalelo",
                column: "EstudianteId");

            migrationBuilder.CreateIndex(
                name: "IX_EstudiantesXParalelo_ParaleloId",
                table: "EstudiantesXParalelo",
                column: "ParaleloId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstudiantesXParalelo");
        }
    }
}
