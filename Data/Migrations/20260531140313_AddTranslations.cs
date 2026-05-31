using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DungeonCompanion.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTranslations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionTranslations",
                table: "Spells",
                type: "jsonb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameTranslations",
                table: "Spells",
                type: "jsonb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionTranslations",
                table: "Skills",
                type: "jsonb",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameTranslations",
                table: "Skills",
                type: "jsonb",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionTranslations",
                table: "Spells");

            migrationBuilder.DropColumn(
                name: "NameTranslations",
                table: "Spells");

            migrationBuilder.DropColumn(
                name: "DescriptionTranslations",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "NameTranslations",
                table: "Skills");
        }
    }
}
