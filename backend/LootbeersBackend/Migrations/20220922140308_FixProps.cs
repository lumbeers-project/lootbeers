using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LootbeersBackend.Migrations
{
    public partial class FixProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lootboxes_SolanaLootboxAddress",
                table: "Lootboxes");

            migrationBuilder.DropColumn(
                name: "TokenReward_MintAddress",
                table: "Rewards");

            migrationBuilder.CreateIndex(
                name: "IX_Lootboxes_SolanaLootboxAddress",
                table: "Lootboxes",
                column: "SolanaLootboxAddress",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lootboxes_SolanaLootboxAddress",
                table: "Lootboxes");

            migrationBuilder.AddColumn<string>(
                name: "TokenReward_MintAddress",
                table: "Rewards",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lootboxes_SolanaLootboxAddress",
                table: "Lootboxes",
                column: "SolanaLootboxAddress");
        }
    }
}
