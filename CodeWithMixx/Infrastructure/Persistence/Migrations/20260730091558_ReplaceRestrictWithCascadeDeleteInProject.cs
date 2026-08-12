using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeWithMixx.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceRestrictWithCascadeDeleteInProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Reservations_ReservationId",
                table: "Projects");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Reservations_ReservationId",
                table: "Projects",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Reservations_ReservationId",
                table: "Projects");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Reservations_ReservationId",
                table: "Projects",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
