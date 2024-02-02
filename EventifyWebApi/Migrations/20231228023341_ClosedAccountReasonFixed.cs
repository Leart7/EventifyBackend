using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventifyWebApi.Migrations
{
    public partial class ClosedAccountReasonFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_ClosedAccountReasons_ClosedAccountReasonId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_ClosedAccountReasonId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ClosedAccountReasonId",
                table: "Events");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClosedAccountReasonId",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_ClosedAccountReasonId",
                table: "Events",
                column: "ClosedAccountReasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_ClosedAccountReasons_ClosedAccountReasonId",
                table: "Events",
                column: "ClosedAccountReasonId",
                principalTable: "ClosedAccountReasons",
                principalColumn: "Id");
        }
    }
}
