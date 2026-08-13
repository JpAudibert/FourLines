using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FourLines.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_facility_schedule_pk_fk_indexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_facility_schedules",
                table: "facility_schedules");

            migrationBuilder.AddUniqueConstraint(
                name: "ak_facility_schedules_facility_id_day_of_week",
                table: "facility_schedules",
                columns: new[] { "facility_id", "day_of_week" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_facility_schedules",
                table: "facility_schedules",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "ak_facility_schedules_facility_id_day_of_week",
                table: "facility_schedules");

            migrationBuilder.DropPrimaryKey(
                name: "pk_facility_schedules",
                table: "facility_schedules");

            migrationBuilder.AddPrimaryKey(
                name: "pk_facility_schedules",
                table: "facility_schedules",
                columns: new[] { "facility_id", "day_of_week" });
        }
    }
}
