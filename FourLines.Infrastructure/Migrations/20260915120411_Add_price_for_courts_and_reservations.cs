using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FourLines.Infrastructure.Migrations;

/// <inheritdoc />
public partial class Add_price_for_courts_and_reservations : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<decimal>(
            name: "amount",
            table: "reservations",
            type: "numeric(19,4)",
            precision: 19,
            scale: 4,
            nullable: false,
            defaultValue: 0m);

        migrationBuilder.AddColumn<string>(
            name: "currency",
            table: "reservations",
            type: "character varying(3)",
            maxLength: 3,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<decimal>(
            name: "amount",
            table: "courts",
            type: "numeric(19,4)",
            precision: 19,
            scale: 4,
            nullable: false,
            defaultValue: 0m);

        migrationBuilder.AddColumn<string>(
            name: "currency",
            table: "courts",
            type: "character varying(3)",
            maxLength: 3,
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "amount",
            table: "reservations");

        migrationBuilder.DropColumn(
            name: "currency",
            table: "reservations");

        migrationBuilder.DropColumn(
            name: "amount",
            table: "courts");

        migrationBuilder.DropColumn(
            name: "currency",
            table: "courts");
    }
}
