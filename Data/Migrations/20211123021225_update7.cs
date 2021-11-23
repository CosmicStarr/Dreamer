using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class update7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GetPhotos_GetProducts_ProductsId",
                table: "GetPhotos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GetPhotos",
                table: "GetPhotos");

            migrationBuilder.RenameTable(
                name: "GetPhotos",
                newName: "Pictures");

            migrationBuilder.RenameIndex(
                name: "IX_GetPhotos_ProductsId",
                table: "Pictures",
                newName: "IX_Pictures_ProductsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pictures",
                table: "Pictures",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_GetProducts_ProductsId",
                table: "Pictures",
                column: "ProductsId",
                principalTable: "GetProducts",
                principalColumn: "productId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_GetProducts_ProductsId",
                table: "Pictures");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pictures",
                table: "Pictures");

            migrationBuilder.RenameTable(
                name: "Pictures",
                newName: "GetPhotos");

            migrationBuilder.RenameIndex(
                name: "IX_Pictures_ProductsId",
                table: "GetPhotos",
                newName: "IX_GetPhotos_ProductsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GetPhotos",
                table: "GetPhotos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GetPhotos_GetProducts_ProductsId",
                table: "GetPhotos",
                column: "ProductsId",
                principalTable: "GetProducts",
                principalColumn: "productId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
