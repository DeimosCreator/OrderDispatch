using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KitchenService.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserIdToIntNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KitchenOrders_Kitchens_KitchenId",
                table: "KitchenOrders");

            migrationBuilder.AlterColumn<int>(
                name: "KitchenId",
                table: "KitchenOrders",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_KitchenOrders_Kitchens_KitchenId",
                table: "KitchenOrders",
                column: "KitchenId",
                principalTable: "Kitchens",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KitchenOrders_Kitchens_KitchenId",
                table: "KitchenOrders");

            migrationBuilder.AlterColumn<int>(
                name: "KitchenId",
                table: "KitchenOrders",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_KitchenOrders_Kitchens_KitchenId",
                table: "KitchenOrders",
                column: "KitchenId",
                principalTable: "Kitchens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
