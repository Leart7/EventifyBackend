using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventifyWebApi.Migrations
{
    public partial class UniqueLike : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Likes_UserId",
                table: "Likes");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserId_EventId",
                table: "Likes",
                columns: new[] { "UserId", "EventId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Likes_UserId_EventId",
                table: "Likes");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_UserId",
                table: "Likes",
                column: "UserId");
        }
    }
}
