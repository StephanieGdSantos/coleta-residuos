using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace coleta_residuos.Migrations
{
    /// <inheritdoc />
    public partial class AddTabelasIniciais : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PontosColeta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Endereco = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    CapacidadeMaximaKg = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PontosColeta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Residuos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Categoria = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    Descricao = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Residuos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Alertas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PontoColetaId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DataAlerta = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    Mensagem = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    Resolvido = table.Column<bool>(type: "NUMBER(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alertas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alertas_PontosColeta_PontoColetaId",
                        column: x => x.PontoColetaId,
                        principalTable: "PontosColeta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ColetasAgendadas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DataAgendada = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    Observacao = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: true),
                    PontoColetaId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColetasAgendadas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ColetasAgendadas_PontosColeta_PontoColetaId",
                        column: x => x.PontoColetaId,
                        principalTable: "PontosColeta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventosColeta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PontoColetaId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DataEvento = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    QuantidadeKg = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    TipoEvento = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventosColeta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventosColeta_PontosColeta_PontoColetaId",
                        column: x => x.PontoColetaId,
                        principalTable: "PontosColeta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PontoColetaResiduo",
                columns: table => new
                {
                    ResiduoId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PontoColetaId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PontoColetaResiduo", x => new { x.PontoColetaId, x.ResiduoId });
                    table.ForeignKey(
                        name: "FK_PontoColetaResiduo_PontosColeta_PontoColetaId",
                        column: x => x.PontoColetaId,
                        principalTable: "PontosColeta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PontoColetaResiduo_Residuos_ResiduoId",
                        column: x => x.ResiduoId,
                        principalTable: "Residuos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_PontoColetaId",
                table: "Alertas",
                column: "PontoColetaId");

            migrationBuilder.CreateIndex(
                name: "IX_ColetasAgendadas_PontoColetaId",
                table: "ColetasAgendadas",
                column: "PontoColetaId");

            migrationBuilder.CreateIndex(
                name: "IX_EventosColeta_PontoColetaId",
                table: "EventosColeta",
                column: "PontoColetaId");

            migrationBuilder.CreateIndex(
                name: "IX_PontoColetaResiduo_ResiduoId",
                table: "PontoColetaResiduo",
                column: "ResiduoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alertas");

            migrationBuilder.DropTable(
                name: "ColetasAgendadas");

            migrationBuilder.DropTable(
                name: "EventosColeta");

            migrationBuilder.DropTable(
                name: "PontoColetaResiduo");

            migrationBuilder.DropTable(
                name: "PontosColeta");

            migrationBuilder.DropTable(
                name: "Residuos");
        }
    }
}
