using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvancedBusinessAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Motos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Placa = table.Column<string>(type: "TEXT", maxLength: 8, nullable: false),
                    Modelo = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                    Ano = table.Column<int>(type: "INTEGER", nullable: false),
                    Chassi = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Latitude = table.Column<double>(type: "REAL", nullable: true),
                    Longitude = table.Column<double>(type: "REAL", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AtualizadoEm = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 160, nullable: false),
                    SenhaHash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manutencoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MotoId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Data = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Custo = table.Column<decimal>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manutencoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Manutencoes_Motos_MotoId",
                        column: x => x.MotoId,
                        principalTable: "Motos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Manutencoes_MotoId",
                table: "Manutencoes",
                column: "MotoId");

            migrationBuilder.CreateIndex(
                name: "IX_Motos_Placa",
                table: "Motos",
                column: "Placa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Manutencoes");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Motos");
        }
    }
}
