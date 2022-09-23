using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LootbeersBackend.Migrations
{
    public partial class FrontendId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FrontId",
                table: "Rewards",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FrontId",
                table: "Rewards");
        }
    }
}
