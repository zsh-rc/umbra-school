using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Umbra.School.Migrations
{
    /// <inheritdoc />
    public partial class AddReportAssessementStatusesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssessmentInfoId",
                table: "UserEnglishWordRatings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReportUserAssessmentStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Total = table.Column<int>(type: "int", nullable: false),
                    NotStarted = table.Column<int>(type: "int", nullable: false),
                    InProgress = table.Column<int>(type: "int", nullable: false),
                    Submitted = table.Column<int>(type: "int", nullable: false),
                    Reviewed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportUserAssessmentStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserEnglishWordRatings_AssessmentInfoId",
                table: "UserEnglishWordRatings",
                column: "AssessmentInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEnglishWordRatings_AssessmentInfos_AssessmentInfoId",
                table: "UserEnglishWordRatings",
                column: "AssessmentInfoId",
                principalTable: "AssessmentInfos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEnglishWordRatings_AssessmentInfos_AssessmentInfoId",
                table: "UserEnglishWordRatings");

            migrationBuilder.DropTable(
                name: "ReportUserAssessmentStatuses");

            migrationBuilder.DropIndex(
                name: "IX_UserEnglishWordRatings_AssessmentInfoId",
                table: "UserEnglishWordRatings");

            migrationBuilder.DropColumn(
                name: "AssessmentInfoId",
                table: "UserEnglishWordRatings");
        }
    }
}
