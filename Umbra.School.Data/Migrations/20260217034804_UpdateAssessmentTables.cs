using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Umbra.School.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAssessmentTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_wordsAssessmentDetails_WordsAssessments_WordsAssessementId",
                table: "wordsAssessmentDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_wordsAssessmentDetails",
                table: "wordsAssessmentDetails");

            migrationBuilder.RenameTable(
                name: "wordsAssessmentDetails",
                newName: "WordsAssessmentDetails");

            migrationBuilder.RenameIndex(
                name: "IX_wordsAssessmentDetails_WordsAssessementId",
                table: "WordsAssessmentDetails",
                newName: "IX_WordsAssessmentDetails_WordsAssessementId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WordsAssessmentDetails",
                table: "WordsAssessmentDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WordsAssessmentDetails_WordsAssessments_WordsAssessementId",
                table: "WordsAssessmentDetails",
                column: "WordsAssessementId",
                principalTable: "WordsAssessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WordsAssessmentDetails_WordsAssessments_WordsAssessementId",
                table: "WordsAssessmentDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WordsAssessmentDetails",
                table: "WordsAssessmentDetails");

            migrationBuilder.RenameTable(
                name: "WordsAssessmentDetails",
                newName: "wordsAssessmentDetails");

            migrationBuilder.RenameIndex(
                name: "IX_WordsAssessmentDetails_WordsAssessementId",
                table: "wordsAssessmentDetails",
                newName: "IX_wordsAssessmentDetails_WordsAssessementId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_wordsAssessmentDetails",
                table: "wordsAssessmentDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_wordsAssessmentDetails_WordsAssessments_WordsAssessementId",
                table: "wordsAssessmentDetails",
                column: "WordsAssessementId",
                principalTable: "WordsAssessments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
