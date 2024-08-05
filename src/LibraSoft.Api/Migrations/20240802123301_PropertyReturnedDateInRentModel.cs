using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraSoft.Api.Migrations
{
    /// <inheritdoc />
    public partial class PropertyReturnedDateInRentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReturnDate",
                table: "Rents",
                newName: "ExpectedReturnDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnedDate",
                table: "Rents",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturnedDate",
                table: "Rents");

            migrationBuilder.RenameColumn(
                name: "ExpectedReturnDate",
                table: "Rents",
                newName: "ReturnDate");
        }
    }
}
