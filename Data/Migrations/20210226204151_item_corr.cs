using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class item_corr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GenderName",
                table: "Items",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GenderName",
                table: "Items");
        }
    }
}
