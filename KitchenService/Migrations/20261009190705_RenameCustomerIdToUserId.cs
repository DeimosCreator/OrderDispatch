using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KitchenService.Migrations
{
    /// <inheritdoc />
    public partial class RenameCustomerIdToUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "KitchenOrders",
                newName: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "KitchenOrders",
                newName: "CustomerId");
        }
    }
}
