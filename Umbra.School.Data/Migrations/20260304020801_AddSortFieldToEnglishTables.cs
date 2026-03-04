using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Umbra.School.Migrations
{
    /// <inheritdoc />
    public partial class AddSortFieldToEnglishTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sort",
                table: "EnglishWords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sort",
                table: "EnglishTranslations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sort",
                table: "EnglishPhrases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sort",
                table: "EnglishWords");

            migrationBuilder.DropColumn(
                name: "Sort",
                table: "EnglishTranslations");

            migrationBuilder.DropColumn(
                name: "Sort",
                table: "EnglishPhrases");
        }
    }
}
