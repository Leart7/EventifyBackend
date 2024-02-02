using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventifyWebApi.Migrations
{
    public partial class UniqueFollower : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Follows_FollowerId",
                table: "Follows");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_FollowerId_FollowedUserId",
                table: "Follows",
                columns: new[] { "FollowerId", "FollowedUserId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Follows_FollowerId_FollowedUserId",
                table: "Follows");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_FollowerId",
                table: "Follows",
                column: "FollowerId");
        }
    }
}
