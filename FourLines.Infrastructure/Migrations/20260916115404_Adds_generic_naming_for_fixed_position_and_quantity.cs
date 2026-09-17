using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FourLines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Adds_generic_naming_for_fixed_position_and_quantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "has_fixed_goal_keeper",
                table: "sports",
                newName: "has_fixed_position");

            migrationBuilder.RenameColumn(
                name: "is_goal_keeper",
                table: "matches_users",
                newName: "is_fixed_position");

            migrationBuilder.AddColumn<int>(
                name: "fixed_position_quantity",
                table: "sports",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fixed_position_quantity",
                table: "sports");

            migrationBuilder.RenameColumn(
                name: "has_fixed_position",
                table: "sports",
                newName: "has_fixed_goal_keeper");

            migrationBuilder.RenameColumn(
                name: "is_fixed_position",
                table: "matches_users",
                newName: "is_goal_keeper");
        }
    }
}
