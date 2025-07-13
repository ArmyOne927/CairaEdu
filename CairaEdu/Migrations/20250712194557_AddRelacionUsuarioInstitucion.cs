using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CairaEdu.Migrations
{
    /// <inheritdoc />
    public partial class AddRelacionUsuarioInstitucion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Institucion_InstitucionId",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Institucion",
                table: "Users",
                column: "InstitucionId",
                principalTable: "Institucion",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Institucion",
                table: "Users");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Institucion_InstitucionId",
                table: "Users",
                column: "InstitucionId",
                principalTable: "Institucion",
                principalColumn: "Id");
        }
    }
}
