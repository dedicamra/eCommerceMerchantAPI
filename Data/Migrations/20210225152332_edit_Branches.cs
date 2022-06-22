using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class edit_Branches : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_City_CityId",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "City");

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Branches",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "CityName",
                table: "Branches",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_City_CityId",
                table: "Branches",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_City_CityId",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "CityName",
                table: "Branches");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "City",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "CityId",
                table: "Branches",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_City_CityId",
                table: "Branches",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
