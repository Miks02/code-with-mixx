using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeWithMixx.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureSubjectToProjectsRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "Projects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_SubjectId",
                table: "Projects",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Subjects_SubjectId",
                table: "Projects",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Subjects_SubjectId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_SubjectId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Projects");
        }
    }
}
