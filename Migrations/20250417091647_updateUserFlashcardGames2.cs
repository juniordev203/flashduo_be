using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class updateUserFlashcardGames2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFlashcardGames_FlashcardSets_FlashcardSetId",
                table: "UserFlashcardGames");

            migrationBuilder.DropIndex(
                name: "IX_UserFlashcardGames_FlashcardSetId",
                table: "UserFlashcardGames");

            migrationBuilder.DropColumn(
                name: "FlashcardSetId",
                table: "UserFlashcardGames");

            migrationBuilder.CreateIndex(
                name: "IX_UserFlashcardGames_SetId",
                table: "UserFlashcardGames",
                column: "SetId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFlashcardGames_FlashcardSets_SetId",
                table: "UserFlashcardGames",
                column: "SetId",
                principalTable: "FlashcardSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFlashcardGames_FlashcardSets_SetId",
                table: "UserFlashcardGames");

            migrationBuilder.DropIndex(
                name: "IX_UserFlashcardGames_SetId",
                table: "UserFlashcardGames");

            migrationBuilder.AddColumn<int>(
                name: "FlashcardSetId",
                table: "UserFlashcardGames",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserFlashcardGames_FlashcardSetId",
                table: "UserFlashcardGames",
                column: "FlashcardSetId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFlashcardGames_FlashcardSets_FlashcardSetId",
                table: "UserFlashcardGames",
                column: "FlashcardSetId",
                principalTable: "FlashcardSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
