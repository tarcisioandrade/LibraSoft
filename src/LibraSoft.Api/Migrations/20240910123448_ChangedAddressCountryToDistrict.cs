using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraSoft.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangedAddressCountryToDistrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address_Country",
                table: "Users",
                newName: "Address_District");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_At",
                table: "Books",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created_At",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "Address_District",
                table: "Users",
                newName: "Address_Country");
        }
    }
}
