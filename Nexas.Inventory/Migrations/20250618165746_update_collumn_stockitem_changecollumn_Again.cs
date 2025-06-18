using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexas.Inventory.Migrations
{
    /// <inheritdoc />
    public partial class update_collumn_stockitem_changecollumn_Again : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProdutoId",
                table: "stockItem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProdutoId",
                table: "stockItem",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
