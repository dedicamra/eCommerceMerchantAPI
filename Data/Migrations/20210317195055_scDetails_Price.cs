using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class scDetails_Price : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "ItemPrice",
                table: "SCDetails",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TotalPrice",
                table: "SCDetails",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemPrice",
                table: "SCDetails");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "SCDetails");
        }
    }
}
