using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class itmbranch_details : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfChanging",
                table: "ItemDetails",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "BranchName",
                table: "ItemBranch",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfAdding",
                table: "ItemBranch",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfChanging",
                table: "ItemDetails");

            migrationBuilder.DropColumn(
                name: "BranchName",
                table: "ItemBranch");

            migrationBuilder.DropColumn(
                name: "DateOfAdding",
                table: "ItemBranch");
        }
    }
}
