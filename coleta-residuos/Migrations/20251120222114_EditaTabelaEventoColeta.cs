using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace coleta_residuos.Migrations
{
    /// <inheritdoc />
    public partial class EditaTabelaEventoColeta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResiduoId",
                table: "EventosColeta",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EventosColeta_ResiduoId",
                table: "EventosColeta",
                column: "ResiduoId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventosColeta_Residuos_ResiduoId",
                table: "EventosColeta",
                column: "ResiduoId",
                principalTable: "Residuos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventosColeta_Residuos_ResiduoId",
                table: "EventosColeta");

            migrationBuilder.DropIndex(
                name: "IX_EventosColeta_ResiduoId",
                table: "EventosColeta");

            migrationBuilder.DropColumn(
                name: "ResiduoId",
                table: "EventosColeta");
        }
    }
}
