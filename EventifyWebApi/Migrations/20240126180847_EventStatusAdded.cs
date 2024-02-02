using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventifyWebApi.Migrations
{
    public partial class EventStatusAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Events");
        }
    }
}
