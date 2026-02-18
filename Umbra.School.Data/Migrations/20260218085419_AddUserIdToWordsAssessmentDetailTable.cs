using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Umbra.School.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToWordsAssessmentDetailTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "WordsAssessmentDetails",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WordId",
                table: "WordsAssessmentDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ForUserIds",
                table: "AssessmentInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ForUserNames",
                table: "AssessmentInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WordsAssessmentDetails_UserId",
                table: "WordsAssessmentDetails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WordsAssessmentDetails_WordId",
                table: "WordsAssessmentDetails",
                column: "WordId");

            migrationBuilder.AddForeignKey(
                name: "FK_WordsAssessmentDetails_EnglishWords_WordId",
                table: "WordsAssessmentDetails",
                column: "WordId",
                principalTable: "EnglishWords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WordsAssessmentDetails_Users_UserId",
                table: "WordsAssessmentDetails",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WordsAssessmentDetails_EnglishWords_WordId",
                table: "WordsAssessmentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_WordsAssessmentDetails_Users_UserId",
                table: "WordsAssessmentDetails");

            migrationBuilder.DropIndex(
                name: "IX_WordsAssessmentDetails_UserId",
                table: "WordsAssessmentDetails");

            migrationBuilder.DropIndex(
                name: "IX_WordsAssessmentDetails_WordId",
                table: "WordsAssessmentDetails");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WordsAssessmentDetails");

            migrationBuilder.DropColumn(
                name: "WordId",
                table: "WordsAssessmentDetails");

            migrationBuilder.DropColumn(
                name: "ForUserIds",
                table: "AssessmentInfos");

            migrationBuilder.DropColumn(
                name: "ForUserNames",
                table: "AssessmentInfos");
        }
    }
}
