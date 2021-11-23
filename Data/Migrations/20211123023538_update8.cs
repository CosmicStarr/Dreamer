using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class update8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pictures_ProductsId",
                table: "Pictures");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_ProductsId",
                table: "Pictures",
                column: "ProductsId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pictures_ProductsId",
                table: "Pictures");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_ProductsId",
                table: "Pictures",
                column: "ProductsId");
        }
    }
}
