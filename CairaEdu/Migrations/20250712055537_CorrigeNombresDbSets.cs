using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CairaEdu.Migrations
{
    /// <inheritdoc />
    public partial class CorrigeNombresDbSets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Provincia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    prov_nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    prov_estado = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provincia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ciudad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ciud_nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ciud_estado = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    ProvinciaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ciudad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ciudad_Provincia",
                        column: x => x.ProvinciaId,
                        principalTable: "Provincia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Institucion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    inst_logo = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    inst_nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    inst_direccion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    inst_dominio = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    inst_ruc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    inst_telefono = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    inst_estado = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CiudadId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Institucion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Institucion_Ciudad",
                        column: x => x.CiudadId,
                        principalTable: "Ciudad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ciudad_ProvinciaId",
                table: "Ciudad",
                column: "ProvinciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Institucion_CiudadId",
                table: "Institucion",
                column: "CiudadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Institucion");

            migrationBuilder.DropTable(
                name: "Ciudad");

            migrationBuilder.DropTable(
                name: "Provincia");
        }
    }
}
