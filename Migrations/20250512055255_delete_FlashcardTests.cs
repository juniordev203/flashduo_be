using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class delete_FlashcardTests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_FlashcardTests_FlashcardTestId",
                table: "Flashcards");

            migrationBuilder.DropTable(
                name: "FlashcardTests");

            migrationBuilder.DropIndex(
                name: "IX_Flashcards_FlashcardTestId",
                table: "Flashcards");

            migrationBuilder.DropColumn(
                name: "FlashcardTestId",
                table: "Flashcards");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FlashcardTestId",
                table: "Flashcards",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FlashcardTests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FlashcardSetId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TestType = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalQuestions = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashcardTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlashcardTests_FlashcardSets_FlashcardSetId",
                        column: x => x.FlashcardSetId,
                        principalTable: "FlashcardSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlashcardTests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Flashcards_FlashcardTestId",
                table: "Flashcards",
                column: "FlashcardTestId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardTests_FlashcardSetId",
                table: "FlashcardTests",
                column: "FlashcardSetId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardTests_UserId",
                table: "FlashcardTests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_FlashcardTests_FlashcardTestId",
                table: "Flashcards",
                column: "FlashcardTestId",
                principalTable: "FlashcardTests",
                principalColumn: "Id");
        }
    }
}
