using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FourLines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_table_relationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_reservations_courts_court_id",
                table: "reservations");

            migrationBuilder.DropForeignKey(
                name: "fk_reservations_users_user_id",
                table: "reservations");

            migrationBuilder.AddForeignKey(
                name: "fk_reservations_courts_court_id",
                table: "reservations",
                column: "court_id",
                principalTable: "courts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_reservations_users_user_id",
                table: "reservations",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_reservations_courts_court_id",
                table: "reservations");

            migrationBuilder.DropForeignKey(
                name: "fk_reservations_users_user_id",
                table: "reservations");

            migrationBuilder.AddForeignKey(
                name: "fk_reservations_courts_court_id",
                table: "reservations",
                column: "court_id",
                principalTable: "courts",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_reservations_users_user_id",
                table: "reservations",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
