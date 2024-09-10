using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraSoft.Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangedNameBookCreatedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "Books",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Books",
                newName: "Created_At");
        }
    }
}
