using Microsoft.EntityFrameworkCore.Migrations;

namespace FptLearningSystem.Data.Migrations
{
    public partial class FixTopicModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Courses_CategoryId",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Topics_CategoryId",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Topics");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_CourseId",
                table: "Topics",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Courses_CourseId",
                table: "Topics",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Courses_CourseId",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Topics_CourseId",
                table: "Topics");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Topics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topics_CategoryId",
                table: "Topics",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Courses_CategoryId",
                table: "Topics",
                column: "CategoryId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
