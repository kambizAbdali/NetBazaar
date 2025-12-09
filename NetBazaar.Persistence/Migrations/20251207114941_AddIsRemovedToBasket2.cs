using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetBazaar.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsRemovedToBasket2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        IF NOT EXISTS (
            SELECT 1
            FROM sys.default_constraints dc
            JOIN sys.columns c ON c.default_object_id = dc.object_id
            JOIN sys.tables t ON t.object_id = c.object_id
            WHERE t.name = 'Baskets' AND c.name = 'IsRemoved'
        )
        BEGIN
            ALTER TABLE [dbo].[Baskets]
            ADD CONSTRAINT [DF_Baskets_IsRemoved] DEFAULT (0) FOR [IsRemoved];
        END
    ");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
