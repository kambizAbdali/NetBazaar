using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetBazaar.Persistence.Data.Migrations.Identity
{
    /// <inheritdoc />
    public partial class InitIdentity11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName",
                schema: "identity",
                table: "Users",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "identity",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "identity",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "identity",
                table: "Users",
                newName: "FullName");
        }
    }
}
