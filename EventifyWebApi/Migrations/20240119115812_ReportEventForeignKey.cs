using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventifyWebApi.Migrations
{
    public partial class ReportEventForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "ReportEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ReportEvents_EventId",
                table: "ReportEvents",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportEvents_Events_EventId",
                table: "ReportEvents",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportEvents_Events_EventId",
                table: "ReportEvents");

            migrationBuilder.DropIndex(
                name: "IX_ReportEvents_EventId",
                table: "ReportEvents");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "ReportEvents");
        }
    }
}
