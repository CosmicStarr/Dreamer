using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class update7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pictures_ProductsId",
                table: "Pictures");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "GetProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_ProductsId",
                table: "Pictures",
                column: "ProductsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pictures_ProductsId",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "GetProducts");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_ProductsId",
                table: "Pictures",
                column: "ProductsId",
                unique: true);
        }
    }
}
