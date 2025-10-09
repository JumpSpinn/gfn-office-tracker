using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficeTracker.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "plannable_days",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_plannable_days", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user_settings",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", nullable: false),
                    HomeOfficeTargetQuoted = table.Column<uint>(type: "INTEGER", nullable: false),
                    HomeOfficeDayCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    HomeOfficeDays = table.Column<string>(type: "TEXT", nullable: false),
                    OfficeTargetQuoted = table.Column<uint>(type: "INTEGER", nullable: false),
                    OfficeDayCount = table.Column<uint>(type: "INTEGER", nullable: false),
                    OfficeDays = table.Column<string>(type: "TEXT", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_settings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "plannable_days");

            migrationBuilder.DropTable(
                name: "user_settings");
        }
    }
}
