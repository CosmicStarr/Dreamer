using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class update17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderTimeSpan",
                table: "GetActualOrders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OrderTimeSpan",
                table: "GetActualOrders",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
