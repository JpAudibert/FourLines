using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FourLines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Adds_match_and_matches_users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "matches",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    reservation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sport_id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_matches", x => x.id);
                    table.ForeignKey(
                        name: "fk_matches_reservations_reservation_id",
                        column: x => x.reservation_id,
                        principalTable: "reservations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_matches_sports_sport_id",
                        column: x => x.sport_id,
                        principalTable: "sports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "matches_users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    match_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_goal_keeper = table.Column<bool>(type: "boolean", nullable: false),
                    user_id1 = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_matches_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_matches_users_matches_match_id",
                        column: x => x.match_id,
                        principalTable: "matches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_matches_users_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_matches_users_users_user_id1",
                        column: x => x.user_id1,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_matches_code",
                table: "matches",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_matches_reservation_id",
                table: "matches",
                column: "reservation_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_matches_sport_id",
                table: "matches",
                column: "sport_id");

            migrationBuilder.CreateIndex(
                name: "ix_matches_users_match_id_user_id",
                table: "matches_users",
                columns: new[] { "match_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_matches_users_user_id",
                table: "matches_users",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_matches_users_user_id1",
                table: "matches_users",
                column: "user_id1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "matches_users");

            migrationBuilder.DropTable(
                name: "matches");
        }
    }
}
