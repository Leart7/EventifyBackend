using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventifyWebApi.Migrations
{
    public partial class ClosedAccountReasonIdFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseAccountReasonId",
                table: "ClosedAccounts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CloseAccountReasonId",
                table: "ClosedAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
