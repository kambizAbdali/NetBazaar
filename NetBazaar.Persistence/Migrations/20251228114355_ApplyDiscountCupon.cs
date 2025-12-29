using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetBazaar.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ApplyDiscountCupon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "Baskets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "DiscountId",
                table: "Baskets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Baskets_DiscountId",
                table: "Baskets",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Baskets_Discounts_DiscountId",
                table: "Baskets",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Baskets_Discounts_DiscountId",
                table: "Baskets");

            migrationBuilder.DropIndex(
                name: "IX_Baskets_DiscountId",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "Baskets");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "Baskets");
        }
    }
}
