using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AnotherUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GetDeliveryMethods",
                columns: table => new
                {
                    DeliveryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveryTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetDeliveryMethods", x => x.DeliveryId);
                });

            migrationBuilder.CreateTable(
                name: "GetActualOrders",
                columns: table => new
                {
                    ActualOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ShippingAddressAddressId = table.Column<int>(type: "int", nullable: true),
                    SpeaiclDeliveryDeliveryId = table.Column<int>(type: "int", nullable: true),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PaymentId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetActualOrders", x => x.ActualOrderId);
                    table.ForeignKey(
                        name: "FK_GetActualOrders_GetAddresses_ShippingAddressAddressId",
                        column: x => x.ShippingAddressAddressId,
                        principalTable: "GetAddresses",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GetActualOrders_GetDeliveryMethods_SpeaiclDeliveryDeliveryId",
                        column: x => x.SpeaiclDeliveryDeliveryId,
                        principalTable: "GetDeliveryMethods",
                        principalColumn: "DeliveryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GetOrderedItems",
                columns: table => new
                {
                    ItemsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    ActualOrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetOrderedItems", x => x.ItemsId);
                    table.ForeignKey(
                        name: "FK_GetOrderedItems_GetActualOrders_ActualOrderId",
                        column: x => x.ActualOrderId,
                        principalTable: "GetActualOrders",
                        principalColumn: "ActualOrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GetActualOrders_ShippingAddressAddressId",
                table: "GetActualOrders",
                column: "ShippingAddressAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_GetActualOrders_SpeaiclDeliveryDeliveryId",
                table: "GetActualOrders",
                column: "SpeaiclDeliveryDeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_GetOrderedItems_ActualOrderId",
                table: "GetOrderedItems",
                column: "ActualOrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GetOrderedItems");

            migrationBuilder.DropTable(
                name: "GetActualOrders");

            migrationBuilder.DropTable(
                name: "GetDeliveryMethods");
        }
    }
}
