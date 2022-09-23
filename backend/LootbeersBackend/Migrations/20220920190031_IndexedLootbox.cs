using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LootbeersBackend.Migrations
{
    public partial class IndexedLootbox : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Lootboxes_SolanaLootboxAddress",
                table: "Lootboxes",
                column: "SolanaLootboxAddress");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lootboxes_SolanaLootboxAddress",
                table: "Lootboxes");
        }
    }
}
