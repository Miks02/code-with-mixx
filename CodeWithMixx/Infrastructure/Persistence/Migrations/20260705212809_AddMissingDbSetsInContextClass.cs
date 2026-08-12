using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeWithMixx.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingDbSetsInContextClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Class_Reservation_ReservationId",
                table: "Class");

            migrationBuilder.DropForeignKey(
                name: "FK_Class_Subject_SubjectId",
                table: "Class");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Admins_AdminId",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Students_StudentId",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Subject_Admins_AdminId",
                table: "Subject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subject",
                table: "Subject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservation",
                table: "Reservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Class",
                table: "Class");

            migrationBuilder.RenameTable(
                name: "Subject",
                newName: "Subjects");

            migrationBuilder.RenameTable(
                name: "Reservation",
                newName: "Reservations");

            migrationBuilder.RenameTable(
                name: "Class",
                newName: "Classes");

            migrationBuilder.RenameIndex(
                name: "IX_Subject_AdminId",
                table: "Subjects",
                newName: "IX_Subjects_AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_StudentId",
                table: "Reservations",
                newName: "IX_Reservations_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_AdminId",
                table: "Reservations",
                newName: "IX_Reservations_AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Class_SubjectId",
                table: "Classes",
                newName: "IX_Classes_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Class_ReservationId",
                table: "Classes",
                newName: "IX_Classes_ReservationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Classes",
                table: "Classes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Reservations_ReservationId",
                table: "Classes",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Subjects_SubjectId",
                table: "Classes",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Admins_AdminId",
                table: "Reservations",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Students_StudentId",
                table: "Reservations",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subjects_Admins_AdminId",
                table: "Subjects",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Reservations_ReservationId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Subjects_SubjectId",
                table: "Classes");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Admins_AdminId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Students_StudentId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Subjects_Admins_AdminId",
                table: "Subjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Classes",
                table: "Classes");

            migrationBuilder.RenameTable(
                name: "Subjects",
                newName: "Subject");

            migrationBuilder.RenameTable(
                name: "Reservations",
                newName: "Reservation");

            migrationBuilder.RenameTable(
                name: "Classes",
                newName: "Class");

            migrationBuilder.RenameIndex(
                name: "IX_Subjects_AdminId",
                table: "Subject",
                newName: "IX_Subject_AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_StudentId",
                table: "Reservation",
                newName: "IX_Reservation_StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_AdminId",
                table: "Reservation",
                newName: "IX_Reservation_AdminId");

            migrationBuilder.RenameIndex(
                name: "IX_Classes_SubjectId",
                table: "Class",
                newName: "IX_Class_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Classes_ReservationId",
                table: "Class",
                newName: "IX_Class_ReservationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subject",
                table: "Subject",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservation",
                table: "Reservation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Class",
                table: "Class",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Class_Reservation_ReservationId",
                table: "Class",
                column: "ReservationId",
                principalTable: "Reservation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Class_Subject_SubjectId",
                table: "Class",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Admins_AdminId",
                table: "Reservation",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Students_StudentId",
                table: "Reservation",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subject_Admins_AdminId",
                table: "Subject",
                column: "AdminId",
                principalTable: "Admins",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
