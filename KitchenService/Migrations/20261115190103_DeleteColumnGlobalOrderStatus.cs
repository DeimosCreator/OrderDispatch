using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KitchenService.Migrations
{
    /// <inheritdoc />
    public partial class DeleteColumnGlobalOrderStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GlobalOrderStatus",
                table: "KitchenOrders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GlobalOrderStatus",
                table: "KitchenOrders",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
