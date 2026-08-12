using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeWithMixx.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexesToClassesAndReservations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountRate",
                table: "Reservations",
                type: "numeric(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,4)",
                oldPrecision: 5,
                oldScale: 4);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_PaymentStatus",
                table: "Reservations",
                column: "PaymentStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ServiceType",
                table: "Reservations",
                column: "ServiceType");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_ClassStatus",
                table: "Classes",
                column: "ClassStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_EndsAt",
                table: "Classes",
                column: "EndsAt");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_StartsAt",
                table: "Classes",
                column: "StartsAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservations_PaymentStatus",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_ServiceType",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Classes_ClassStatus",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_EndsAt",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_StartsAt",
                table: "Classes");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountRate",
                table: "Reservations",
                type: "numeric(5,4)",
                precision: 5,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,2)",
                oldPrecision: 5,
                oldScale: 2);
        }
    }
}
