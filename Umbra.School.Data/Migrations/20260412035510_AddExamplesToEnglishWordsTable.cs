using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Umbra.School.Migrations
{
    /// <inheritdoc />
    public partial class AddExamplesToEnglishWordsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Examples",
                table: "EnglishWords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChineseClassicals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sentence = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Keyword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KeywordMeaning = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChineseClassicals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserChineseClassicalRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClassicalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    LastReviewed = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChineseClassicalRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserChineseClassicalRatings_ChineseClassicals_ClassicalId",
                        column: x => x.ClassicalId,
                        principalTable: "ChineseClassicals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChineseClassicalRatings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserChineseClassicalRatings_ClassicalId",
                table: "UserChineseClassicalRatings",
                column: "ClassicalId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChineseClassicalRatings_UserId",
                table: "UserChineseClassicalRatings",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserChineseClassicalRatings");

            migrationBuilder.DropTable(
                name: "ChineseClassicals");

            migrationBuilder.DropColumn(
                name: "Examples",
                table: "EnglishWords");
        }
    }
}
