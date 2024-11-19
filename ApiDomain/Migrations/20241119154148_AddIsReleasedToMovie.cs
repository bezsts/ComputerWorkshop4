using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiDomain.Migrations
{
    /// <inheritdoc />
    public partial class AddIsReleasedToMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "ReleaseDate",
                table: "Movie",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(2024, 11, 19),
                oldClrType: typeof(DateOnly),
                oldType: "TEXT",
                oldDefaultValue: new DateOnly(2024, 11, 12));

            migrationBuilder.AddColumn<bool>(
                name: "IsReleased",
                table: "Movie",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReleased",
                table: "Movie");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ReleaseDate",
                table: "Movie",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(2024, 11, 12),
                oldClrType: typeof(DateOnly),
                oldType: "TEXT",
                oldDefaultValue: new DateOnly(2024, 11, 19));
        }
    }
}
