using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventifyWebApi.Migrations
{
    public partial class TagModelForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Events_EventId",
                table: "Tags");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Tags",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Events_EventId",
                table: "Tags",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Events_EventId",
                table: "Tags");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Tags",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Events_EventId",
                table: "Tags",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id");
        }
    }
}
