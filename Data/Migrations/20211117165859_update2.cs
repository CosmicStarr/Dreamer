using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GetProducts_Brands_BrandId",
                table: "GetProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_GetProducts_Categories_CategoryCatId",
                table: "GetProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Brands",
                table: "Brands");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "GetCategories");

            migrationBuilder.RenameTable(
                name: "Brands",
                newName: "GetBrands");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GetCategories",
                table: "GetCategories",
                column: "CatId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GetBrands",
                table: "GetBrands",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_GetProducts_GetBrands_BrandId",
                table: "GetProducts",
                column: "BrandId",
                principalTable: "GetBrands",
                principalColumn: "BrandId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GetProducts_GetCategories_CategoryCatId",
                table: "GetProducts",
                column: "CategoryCatId",
                principalTable: "GetCategories",
                principalColumn: "CatId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GetProducts_GetBrands_BrandId",
                table: "GetProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_GetProducts_GetCategories_CategoryCatId",
                table: "GetProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GetCategories",
                table: "GetCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GetBrands",
                table: "GetBrands");

            migrationBuilder.RenameTable(
                name: "GetCategories",
                newName: "Categories");

            migrationBuilder.RenameTable(
                name: "GetBrands",
                newName: "Brands");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "CatId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brands",
                table: "Brands",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_GetProducts_Brands_BrandId",
                table: "GetProducts",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "BrandId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GetProducts_Categories_CategoryCatId",
                table: "GetProducts",
                column: "CategoryCatId",
                principalTable: "Categories",
                principalColumn: "CatId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
