using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventifyWebApi.Migrations
{
    public partial class EventAgendFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventAgends_Events_EventId",
                table: "EventAgends");

            migrationBuilder.DropColumn(
                name: "Event",
                table: "EventAgends");

            migrationBuilder.AlterColumn<string>(
                name: "Speaker",
                table: "EventAgends",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "EventAgends",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "EventAgends",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_EventAgends_Events_EventId",
                table: "EventAgends",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventAgends_Events_EventId",
                table: "EventAgends");

            migrationBuilder.AlterColumn<string>(
                name: "Speaker",
                table: "EventAgends",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "EventAgends",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "EventAgends",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Event",
                table: "EventAgends",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_EventAgends_Events_EventId",
                table: "EventAgends",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");
        }
    }
}
