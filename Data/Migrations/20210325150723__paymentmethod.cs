using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class _paymentmethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethodName",
                table: "Paymentmethod");

            migrationBuilder.DropColumn(
                name: "Successful",
                table: "Paymentmethod");

            migrationBuilder.AddColumn<string>(
                name: "CVV",
                table: "Paymentmethod",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "Paymentmethod",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Paymentmethod",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CVV",
                table: "Paymentmethod");

            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "Paymentmethod");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Paymentmethod");

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethodName",
                table: "Paymentmethod",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Successful",
                table: "Paymentmethod",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
