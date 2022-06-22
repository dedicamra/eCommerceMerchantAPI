using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class changeUserInCoupon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_Users_UserId",
                table: "Coupons");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_UserId",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Coupons");

            migrationBuilder.AddColumn<int>(
                name: "UsersMerchantId",
                table: "Coupons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_UsersMerchantId",
                table: "Coupons",
                column: "UsersMerchantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_UsersMerchants_UsersMerchantId",
                table: "Coupons",
                column: "UsersMerchantId",
                principalTable: "UsersMerchants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_UsersMerchants_UsersMerchantId",
                table: "Coupons");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_UsersMerchantId",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "UsersMerchantId",
                table: "Coupons");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Coupons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_UserId",
                table: "Coupons",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_Users_UserId",
                table: "Coupons",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
