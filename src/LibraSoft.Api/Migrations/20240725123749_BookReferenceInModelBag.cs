using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraSoft.Api.Migrations
{
    /// <inheritdoc />
    public partial class BookReferenceInModelBag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Bags_BookId",
                table: "Bags",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bags_Books_BookId",
                table: "Bags",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bags_Books_BookId",
                table: "Bags");

            migrationBuilder.DropIndex(
                name: "IX_Bags_BookId",
                table: "Bags");
        }
    }
}
