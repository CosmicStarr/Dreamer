using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class update15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
 

 
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "GetCartItems");

            migrationBuilder.DropTable(
                name: "GetMappedProducts");

            migrationBuilder.DropTable(
                name: "GetOrderedItems");

            migrationBuilder.DropTable(
                name: "GetPhotos");

            migrationBuilder.DropTable(
                name: "GetProducts");

            migrationBuilder.DropTable(
                name: "GetUserAddresses");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "GetActualOrders");

            migrationBuilder.DropTable(
                name: "GetBrands");

            migrationBuilder.DropTable(
                name: "GetCategories");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "GetDeliveryMethods");
        }
    }
}
