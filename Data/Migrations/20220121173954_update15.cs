using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class update15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "GetOrderedItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "GetOrderedItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
