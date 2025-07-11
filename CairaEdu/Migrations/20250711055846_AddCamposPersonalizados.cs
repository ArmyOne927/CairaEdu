using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CairaEdu.Migrations
{
    /// <inheritdoc />
    public partial class AddCamposPersonalizados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Apellidos",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Documento",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Users",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaNacimiento",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Foto",
                table: "Users",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombres",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoDocumentoId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TipoDocumento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tpdoc_descripcion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    tpdoc_estado = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDocumento", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_TipoDocumentoId",
                table: "Users",
                column: "TipoDocumentoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_TipoDocumento_TipoDocumentoId",
                table: "Users",
                column: "TipoDocumentoId",
                principalTable: "TipoDocumento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_TipoDocumento_TipoDocumentoId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "TipoDocumento");

            migrationBuilder.DropIndex(
                name: "IX_Users_TipoDocumentoId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Apellidos",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Documento",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FechaNacimiento",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Foto",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Nombres",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TipoDocumentoId",
                table: "Users");
        }
    }
}
