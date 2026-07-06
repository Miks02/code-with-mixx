using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeWithMixx.Migrations
{
    /// <inheritdoc />
    public partial class AddBonusAndDiscountRatePropertiesInReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Bonus",
                table: "Reservations",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountRate",
                table: "Reservations",
                type: "numeric(5,4)",
                precision: 5,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Reservations_Bonus_Positive",
                table: "Reservations",
                sql: "\"Bonus\" >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Reservations_DiscountRate_LessThan100",
                table: "Reservations",
                sql: "\"DiscountRate\" <= 100");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Reservations_DiscountRate_Positive",
                table: "Reservations",
                sql: "\"DiscountRate\" >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Reservations_Bonus_Positive",
                table: "Reservations");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Reservations_DiscountRate_LessThan100",
                table: "Reservations");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Reservations_DiscountRate_Positive",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Bonus",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "DiscountRate",
                table: "Reservations");
        }
    }
}
