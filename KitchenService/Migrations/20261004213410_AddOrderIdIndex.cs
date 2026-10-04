using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KitchenService.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderIdIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_KitchenOrders_OrderId",
                table: "KitchenOrders",
                column: "OrderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_KitchenOrders_OrderId",
                table: "KitchenOrders");
        }
    }
}
