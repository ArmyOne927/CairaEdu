using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CairaEdu.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintToMateriaProfesor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MateriaProfesor_mp_mat_id",
                table: "MateriaProfesor");

            migrationBuilder.CreateIndex(
                name: "UX_MateriaProfesor_Materia_User",
                table: "MateriaProfesor",
                columns: new[] { "mp_mat_id", "mp_user_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_MateriaProfesor_Materia_User",
                table: "MateriaProfesor");

            migrationBuilder.CreateIndex(
                name: "IX_MateriaProfesor_mp_mat_id",
                table: "MateriaProfesor",
                column: "mp_mat_id");
        }
    }
}
