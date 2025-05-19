using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class updateuseranswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_ExamQuestions_QuestionId",
                table: "UserAnswers");

            migrationBuilder.AddColumn<int>(
                name: "ExamQuestionId",
                table: "UserAnswers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_ExamQuestionId",
                table: "UserAnswers",
                column: "ExamQuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_ExamQuestions_ExamQuestionId",
                table: "UserAnswers",
                column: "ExamQuestionId",
                principalTable: "ExamQuestions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_Questions_QuestionId",
                table: "UserAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_ExamQuestions_ExamQuestionId",
                table: "UserAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_Questions_QuestionId",
                table: "UserAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UserAnswers_ExamQuestionId",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "ExamQuestionId",
                table: "UserAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_ExamQuestions_QuestionId",
                table: "UserAnswers",
                column: "QuestionId",
                principalTable: "ExamQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
