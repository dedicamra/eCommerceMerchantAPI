using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class ResetTokenUserMerchant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "UsersMerchants",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResetToken",
                table: "UsersMerchants",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpires",
                table: "UsersMerchants",
                nullable: true);

            

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "UsersMerchants");

            migrationBuilder.DropColumn(
                name: "ResetToken",
                table: "UsersMerchants");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpires",
                table: "UsersMerchants");

        }
    }
}
