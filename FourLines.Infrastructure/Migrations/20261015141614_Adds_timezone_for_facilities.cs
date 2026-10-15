using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FourLines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Adds_timezone_for_facilities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "time_zone_id",
                table: "facilities",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "time_zone_id",
                table: "facilities");
        }
    }
}
