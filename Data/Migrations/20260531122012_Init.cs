using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DungeonCompanion.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CampaignId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    RuleSetId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Experience = table.Column<int>(type: "integer", nullable: false),
                    CurrentHp = table.Column<int>(type: "integer", nullable: false),
                    MaxHp = table.Column<int>(type: "integer", nullable: false),
                    Attributes = table.Column<string>(type: "jsonb", nullable: false),
                    SpellIds = table.Column<string>(type: "jsonb", nullable: false),
                    SkillIds = table.Column<string>(type: "jsonb", nullable: false),
                    FeatIds = table.Column<string>(type: "jsonb", nullable: false),
                    WeaponIds = table.Column<string>(type: "jsonb", nullable: false),
                    SystemData = table.Column<string>(type: "jsonb", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConflictState = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    FieldMeta = table.Column<string>(type: "jsonb", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    PortraitUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    RuleSetId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    KeyAbility = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    TrainedOnly = table.Column<bool>(type: "boolean", nullable: false),
                    ArmorCheckPenalty = table.Column<bool>(type: "boolean", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DatasetVersion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Spells",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    RuleSetId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    School = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CastingTime = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Range = table.Column<int>(type: "integer", nullable: false),
                    Component = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Duration = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    SavingThrow = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Tags = table.Column<string>(type: "jsonb", nullable: false),
                    DatasetVersion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spells", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characters_CampaignId_OwnerId_Name",
                table: "Characters",
                columns: new[] { "CampaignId", "OwnerId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_RuleSetId_Name",
                table: "Skills",
                columns: new[] { "RuleSetId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Spells_RuleSetId_Name",
                table: "Spells",
                columns: new[] { "RuleSetId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Spells");
        }
    }
}
