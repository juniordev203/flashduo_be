using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class update_user_connect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserExamFavorites_Users_UserId",
                table: "UserExamFavorites");

            migrationBuilder.AddForeignKey(
                name: "FK_UserExamFavorites_Users_UserId",
                table: "UserExamFavorites",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserExamFavorites_Users_UserId",
                table: "UserExamFavorites");

            migrationBuilder.AddForeignKey(
                name: "FK_UserExamFavorites_Users_UserId",
                table: "UserExamFavorites",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
