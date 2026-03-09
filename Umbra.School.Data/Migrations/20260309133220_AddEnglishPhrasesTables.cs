using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Umbra.School.Migrations
{
    /// <inheritdoc />
    public partial class AddEnglishPhrasesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserEnglishPhraseRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhraseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    LastReviewed = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEnglishPhraseRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserEnglishPhraseRatings_EnglishPhrases_PhraseId",
                        column: x => x.PhraseId,
                        principalTable: "EnglishPhrases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserEnglishPhraseRatings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserEnglishPhraseRatings_PhraseId",
                table: "UserEnglishPhraseRatings",
                column: "PhraseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEnglishPhraseRatings_UserId",
                table: "UserEnglishPhraseRatings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserEnglishPhraseRatings");
        }
    }
}
