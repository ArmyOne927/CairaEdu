using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CairaEdu.Migrations
{
    /// <inheritdoc />
    public partial class ExtappUserInst : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InstitucionId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_InstitucionId",
                table: "Users",
                column: "InstitucionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Institucion_InstitucionId",
                table: "Users",
                column: "InstitucionId",
                principalTable: "Institucion",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Institucion_InstitucionId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_InstitucionId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "InstitucionId",
                table: "Users");
        }
    }
}
