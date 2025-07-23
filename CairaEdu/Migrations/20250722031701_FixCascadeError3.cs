using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CairaEdu.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadeError3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EstudiantesXParalelo_EstudianteId",
                table: "EstudiantesXParalelo");

            migrationBuilder.CreateTable(
                name: "HorarioParalelo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParaleloId = table.Column<int>(type: "int", nullable: false),
                    hor_mat_id = table.Column<int>(type: "int", nullable: false),
                    hor_inicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    hor_fin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    hor_matprof_id = table.Column<int>(type: "int", nullable: true),
                    hor_aula = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    hor_estado = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HorarioParalelo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HorarioParalelo_Materia",
                        column: x => x.hor_mat_id,
                        principalTable: "Materia",
                        principalColumn: "mat_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HorarioParalelo_MateriaProfesor",
                        column: x => x.hor_matprof_id,
                        principalTable: "MateriaProfesor",
                        principalColumn: "mp_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HorarioParalelo_Paralelo",
                        column: x => x.ParaleloId,
                        principalTable: "Paralelo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstudiantesXParalelo_EstudianteId",
                table: "EstudiantesXParalelo",
                column: "EstudianteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HorarioParalelo_hor_mat_id",
                table: "HorarioParalelo",
                column: "hor_mat_id");

            migrationBuilder.CreateIndex(
                name: "IX_HorarioParalelo_hor_matprof_id",
                table: "HorarioParalelo",
                column: "hor_matprof_id");

            migrationBuilder.CreateIndex(
                name: "IX_HorarioParalelo_ParaleloId",
                table: "HorarioParalelo",
                column: "ParaleloId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HorarioParalelo");

            migrationBuilder.DropIndex(
                name: "IX_EstudiantesXParalelo_EstudianteId",
                table: "EstudiantesXParalelo");

            migrationBuilder.CreateIndex(
                name: "IX_EstudiantesXParalelo_EstudianteId",
                table: "EstudiantesXParalelo",
                column: "EstudianteId");
        }
    }
}
