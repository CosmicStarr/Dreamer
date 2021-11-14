using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_GetProducts_ProductID",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GetProducts");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "Pictures",
                newName: "ProductsId");

            migrationBuilder.RenameIndex(
                name: "IX_Pictures_ProductID",
                table: "Pictures",
                newName: "IX_Pictures_ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_GetProducts_ProductsId",
                table: "Pictures",
                column: "ProductsId",
                principalTable: "GetProducts",
                principalColumn: "ProdId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_GetProducts_ProductsId",
                table: "Pictures");

            migrationBuilder.RenameColumn(
                name: "ProductsId",
                table: "Pictures",
                newName: "ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_Pictures_ProductsId",
                table: "Pictures",
                newName: "IX_Pictures_ProductID");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "GetProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_GetProducts_ProductID",
                table: "Pictures",
                column: "ProductID",
                principalTable: "GetProducts",
                principalColumn: "ProdId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
