using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class update8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "GetProducts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "GetProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
