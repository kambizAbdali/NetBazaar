using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetBazaar.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class init101 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catalogs_CatalogBrands_CatalogBrandId",
                table: "Catalogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Catalogs_CatalogTypes_CatalogTypeId",
                table: "Catalogs");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Catalogs",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddForeignKey(
                name: "FK_Catalogs_CatalogBrands_CatalogBrandId",
                table: "Catalogs",
                column: "CatalogBrandId",
                principalTable: "CatalogBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Catalogs_CatalogTypes_CatalogTypeId",
                table: "Catalogs",
                column: "CatalogTypeId",
                principalTable: "CatalogTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catalogs_CatalogBrands_CatalogBrandId",
                table: "Catalogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Catalogs_CatalogTypes_CatalogTypeId",
                table: "Catalogs");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Catalogs",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000);

            migrationBuilder.AddForeignKey(
                name: "FK_Catalogs_CatalogBrands_CatalogBrandId",
                table: "Catalogs",
                column: "CatalogBrandId",
                principalTable: "CatalogBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Catalogs_CatalogTypes_CatalogTypeId",
                table: "Catalogs",
                column: "CatalogTypeId",
                principalTable: "CatalogTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
