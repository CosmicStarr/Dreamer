using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class update11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GetCartItems");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "GetOrderedItems",
                newName: "PhotoUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoUrl",
                table: "GetOrderedItems",
                newName: "ImageUrl");

            migrationBuilder.CreateTable(
                name: "GetCartItems",
                columns: table => new
                {
                    CartItemsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal (18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetCartItems", x => x.CartItemsId);
                });
        }
    }
}
