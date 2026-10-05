using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KitchenService.Migrations
{
    /// <inheritdoc />
    public partial class MergeOrderReadModelIntoKitchenOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "KitchenOrders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GlobalOrderStatus",
                table: "KitchenOrders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "KitchenOrders",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "KitchenOrders");

            migrationBuilder.DropColumn(
                name: "GlobalOrderStatus",
                table: "KitchenOrders");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "KitchenOrders");
        }
    }
}
