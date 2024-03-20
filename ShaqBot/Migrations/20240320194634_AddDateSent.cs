using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShaqBot.Migrations
{
    /// <inheritdoc />
    public partial class AddDateSent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateSent",
                table: "LastMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateSent",
                table: "LastMessages");
        }
    }
}
