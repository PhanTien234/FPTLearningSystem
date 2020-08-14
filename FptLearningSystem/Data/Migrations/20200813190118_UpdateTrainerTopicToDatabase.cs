using Microsoft.EntityFrameworkCore.Migrations;

namespace FptLearningSystem.Data.Migrations
{
    public partial class UpdateTrainerTopicToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_trainerTopics_Topics_TopicId",
                table: "trainerTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_trainerTopics_AspNetUsers_TrainerId",
                table: "trainerTopics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_trainerTopics",
                table: "trainerTopics");

            migrationBuilder.RenameTable(
                name: "trainerTopics",
                newName: "TrainerTopics");

            migrationBuilder.RenameIndex(
                name: "IX_trainerTopics_TrainerId",
                table: "TrainerTopics",
                newName: "IX_TrainerTopics_TrainerId");

            migrationBuilder.RenameIndex(
                name: "IX_trainerTopics_TopicId",
                table: "TrainerTopics",
                newName: "IX_TrainerTopics_TopicId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TrainerTopics",
                table: "TrainerTopics",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainerTopics_Topics_TopicId",
                table: "TrainerTopics",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainerTopics_AspNetUsers_TrainerId",
                table: "TrainerTopics",
                column: "TrainerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainerTopics_Topics_TopicId",
                table: "TrainerTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainerTopics_AspNetUsers_TrainerId",
                table: "TrainerTopics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TrainerTopics",
                table: "TrainerTopics");

            migrationBuilder.RenameTable(
                name: "TrainerTopics",
                newName: "trainerTopics");

            migrationBuilder.RenameIndex(
                name: "IX_TrainerTopics_TrainerId",
                table: "trainerTopics",
                newName: "IX_trainerTopics_TrainerId");

            migrationBuilder.RenameIndex(
                name: "IX_TrainerTopics_TopicId",
                table: "trainerTopics",
                newName: "IX_trainerTopics_TopicId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_trainerTopics",
                table: "trainerTopics",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_trainerTopics_Topics_TopicId",
                table: "trainerTopics",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_trainerTopics_AspNetUsers_TrainerId",
                table: "trainerTopics",
                column: "TrainerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
