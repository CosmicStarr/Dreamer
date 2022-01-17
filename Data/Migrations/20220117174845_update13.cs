using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class update13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GetActualOrders_GetDeliveryMethods_SpeaiclDeliveryDeliveryId",
                table: "GetActualOrders");

            migrationBuilder.DropTable(
                name: "GetDeliveryMethods");

            migrationBuilder.DropIndex(
                name: "IX_GetActualOrders_SpeaiclDeliveryDeliveryId",
                table: "GetActualOrders");

            migrationBuilder.DropColumn(
                name: "SpeaiclDeliveryDeliveryId",
                table: "GetActualOrders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpeaiclDeliveryDeliveryId",
                table: "GetActualOrders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GetDeliveryMethods",
                columns: table => new
                {
                    DeliveryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetDeliveryMethods", x => x.DeliveryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GetActualOrders_SpeaiclDeliveryDeliveryId",
                table: "GetActualOrders",
                column: "SpeaiclDeliveryDeliveryId");

            migrationBuilder.AddForeignKey(
                name: "FK_GetActualOrders_GetDeliveryMethods_SpeaiclDeliveryDeliveryId",
                table: "GetActualOrders",
                column: "SpeaiclDeliveryDeliveryId",
                principalTable: "GetDeliveryMethods",
                principalColumn: "DeliveryId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
