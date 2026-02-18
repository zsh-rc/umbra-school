using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Umbra.School.Migrations
{
    /// <inheritdoc />
    public partial class AddAssessmentTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssessmentInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpectDuration = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotebookInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotebookInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssessmentResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssessmentInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Score = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssessmentResults_AssessmentInfos_AssessmentInfoId",
                        column: x => x.AssessmentInfoId,
                        principalTable: "AssessmentInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssessmentResults_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WordsAssessments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssessmentInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Scope = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NotebookInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WordCount = table.Column<int>(type: "int", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartIndex = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordsAssessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordsAssessments_AssessmentInfos_AssessmentInfoId",
                        column: x => x.AssessmentInfoId,
                        principalTable: "AssessmentInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WordsAssessments_NotebookInfos_NotebookInfoId",
                        column: x => x.NotebookInfoId,
                        principalTable: "NotebookInfos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "wordsAssessmentDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Word = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WordsAssessementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Meaning = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Correct = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wordsAssessmentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_wordsAssessmentDetails_WordsAssessments_WordsAssessementId",
                        column: x => x.WordsAssessementId,
                        principalTable: "WordsAssessments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentResults_AssessmentInfoId",
                table: "AssessmentResults",
                column: "AssessmentInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentResults_UserId",
                table: "AssessmentResults",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_wordsAssessmentDetails_WordsAssessementId",
                table: "wordsAssessmentDetails",
                column: "WordsAssessementId");

            migrationBuilder.CreateIndex(
                name: "IX_WordsAssessments_AssessmentInfoId",
                table: "WordsAssessments",
                column: "AssessmentInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_WordsAssessments_NotebookInfoId",
                table: "WordsAssessments",
                column: "NotebookInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssessmentResults");

            migrationBuilder.DropTable(
                name: "wordsAssessmentDetails");

            migrationBuilder.DropTable(
                name: "WordsAssessments");

            migrationBuilder.DropTable(
                name: "AssessmentInfos");

            migrationBuilder.DropTable(
                name: "NotebookInfos");
        }
    }
}
