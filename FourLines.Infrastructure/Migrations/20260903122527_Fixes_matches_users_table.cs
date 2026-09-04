using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FourLines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fixes_matches_users_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_matches_users_users_user_id1",
                table: "matches_users");

            migrationBuilder.DropIndex(
                name: "ix_matches_users_user_id1",
                table: "matches_users");

            migrationBuilder.DropColumn(
                name: "user_id1",
                table: "matches_users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "user_id1",
                table: "matches_users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_matches_users_user_id1",
                table: "matches_users",
                column: "user_id1");

            migrationBuilder.AddForeignKey(
                name: "fk_matches_users_users_user_id1",
                table: "matches_users",
                column: "user_id1",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
