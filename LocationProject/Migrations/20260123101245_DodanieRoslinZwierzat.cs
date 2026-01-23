using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LocationProject.Migrations
{
    /// <inheritdoc />
    public partial class DodanieRoslinZwierzat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rosliny",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LokacjaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rosliny", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rosliny_Lokacje_LokacjaId",
                        column: x => x.LokacjaId,
                        principalTable: "Lokacje",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Zwierzeta",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LokacjaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zwierzeta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zwierzeta_Lokacje_LokacjaId",
                        column: x => x.LokacjaId,
                        principalTable: "Lokacje",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rosliny_LokacjaId",
                table: "Rosliny",
                column: "LokacjaId");

            migrationBuilder.CreateIndex(
                name: "IX_Zwierzeta_LokacjaId",
                table: "Zwierzeta",
                column: "LokacjaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rosliny");

            migrationBuilder.DropTable(
                name: "Zwierzeta");
        }
    }
}
