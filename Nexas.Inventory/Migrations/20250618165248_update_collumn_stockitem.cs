using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nexas.Inventory.Migrations
{
    /// <inheritdoc />
    public partial class update_collumn_stockitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_stockItemEntities_Products_ProductId",
                table: "stockItemEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_stockItemEntities_Stores_StoreId",
                table: "stockItemEntities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_stockItemEntities",
                table: "stockItemEntities");

            migrationBuilder.RenameTable(
                name: "stockItemEntities",
                newName: "stockItem");

            migrationBuilder.RenameIndex(
                name: "IX_stockItemEntities_StoreId",
                table: "stockItem",
                newName: "IX_stockItem_StoreId");

            migrationBuilder.RenameIndex(
                name: "IX_stockItemEntities_ProductId",
                table: "stockItem",
                newName: "IX_stockItem_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_stockItem",
                table: "stockItem",
                column: "StockItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_stockItem_Products_ProductId",
                table: "stockItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_stockItem_Stores_StoreId",
                table: "stockItem",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_stockItem_Products_ProductId",
                table: "stockItem");

            migrationBuilder.DropForeignKey(
                name: "FK_stockItem_Stores_StoreId",
                table: "stockItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_stockItem",
                table: "stockItem");

            migrationBuilder.RenameTable(
                name: "stockItem",
                newName: "stockItemEntities");

            migrationBuilder.RenameIndex(
                name: "IX_stockItem_StoreId",
                table: "stockItemEntities",
                newName: "IX_stockItemEntities_StoreId");

            migrationBuilder.RenameIndex(
                name: "IX_stockItem_ProductId",
                table: "stockItemEntities",
                newName: "IX_stockItemEntities_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_stockItemEntities",
                table: "stockItemEntities",
                column: "StockItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_stockItemEntities_Products_ProductId",
                table: "stockItemEntities",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_stockItemEntities_Stores_StoreId",
                table: "stockItemEntities",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
