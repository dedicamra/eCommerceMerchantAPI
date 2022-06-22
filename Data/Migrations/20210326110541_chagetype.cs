using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class chagetype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "StartPrice",
                table: "Items",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StartPrice",
                table: "Items",
                type: "int",
                nullable: false,
                oldClrType: typeof(float));
        }
    }
}
