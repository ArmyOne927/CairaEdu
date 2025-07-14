using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CairaEdu.Migrations
{
    /// <inheritdoc />
    public partial class MateriasYProfesores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ciud_nombre",
                table: "Ciudad",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateTable(
                name: "Materia",
                columns: table => new
                {
                    mat_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mat_nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    mat_competencias = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    mat_objetivos = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    mat_imagen = table.Column<byte[]>(type: "varbinary(64)", maxLength: 64, nullable: true),
                    mat_estado = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materia", x => x.mat_id);
                });

            migrationBuilder.CreateTable(
                name: "MateriaProfesor",
                columns: table => new
                {
                    mp_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mp_user_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    mp_mat_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MateriaProfesor", x => x.mp_id);
                    table.ForeignKey(
                        name: "FK_MateriaProfesor_Materia_mp_mat_id",
                        column: x => x.mp_mat_id,
                        principalTable: "Materia",
                        principalColumn: "mat_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MateriaProfesor_Users_mp_user_id",
                        column: x => x.mp_user_id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MateriaProfesor_mp_mat_id",
                table: "MateriaProfesor",
                column: "mp_mat_id");

            migrationBuilder.CreateIndex(
                name: "IX_MateriaProfesor_mp_user_id",
                table: "MateriaProfesor",
                column: "mp_user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MateriaProfesor");

            migrationBuilder.DropTable(
                name: "Materia");

            migrationBuilder.AlterColumn<string>(
                name: "ciud_nombre",
                table: "Ciudad",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
