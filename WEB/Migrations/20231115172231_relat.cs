using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB.Migrations
{
    /// <inheritdoc />
    public partial class relat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_coursesNames_CourseId",
                table: "coursesNames",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_coursesNames_CeditCourses_CourseId",
                table: "coursesNames",
                column: "CourseId",
                principalTable: "CeditCourses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_coursesNames_CeditCourses_CourseId",
                table: "coursesNames");

            migrationBuilder.DropIndex(
                name: "IX_coursesNames_CourseId",
                table: "coursesNames");
        }
    }
}
