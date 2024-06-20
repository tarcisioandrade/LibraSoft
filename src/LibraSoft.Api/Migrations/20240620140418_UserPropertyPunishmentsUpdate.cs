using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraSoft.Api.Migrations
{
    /// <inheritdoc />
    public partial class UserPropertyPunishmentsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PunishmentDetails",
                table: "PunishmentDetails");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "PunishmentDetails",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PunishmentDetails",
                table: "PunishmentDetails",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PunishmentDetails_UserId",
                table: "PunishmentDetails",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PunishmentDetails",
                table: "PunishmentDetails");

            migrationBuilder.DropIndex(
                name: "IX_PunishmentDetails_UserId",
                table: "PunishmentDetails");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "PunishmentDetails",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PunishmentDetails",
                table: "PunishmentDetails",
                columns: new[] { "UserId", "Id" });
        }
    }
}
