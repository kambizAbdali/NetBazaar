using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetBazaar.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class init100 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CatalogBrandId1",
                table: "Catalogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_CatalogBrandId1",
                table: "Catalogs",
                column: "CatalogBrandId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Catalogs_CatalogBrands_CatalogBrandId1",
                table: "Catalogs",
                column: "CatalogBrandId1",
                principalTable: "CatalogBrands",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catalogs_CatalogBrands_CatalogBrandId1",
                table: "Catalogs");

            migrationBuilder.DropIndex(
                name: "IX_Catalogs_CatalogBrandId1",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "CatalogBrandId1",
                table: "Catalogs");
        }
    }
}
