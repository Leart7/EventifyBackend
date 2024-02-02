using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventifyWebApi.Migrations
{
    public partial class FollowersModelFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Follows_AspNetUsers_UserId",
                table: "Follows");

            migrationBuilder.DropForeignKey(
                name: "FK_Follows_Events_EventId",
                table: "Follows");

            migrationBuilder.DropIndex(
                name: "IX_Follows_EventId",
                table: "Follows");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Follows");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Follows",
                newName: "FollowerId");

            migrationBuilder.RenameIndex(
                name: "IX_Follows_UserId",
                table: "Follows",
                newName: "IX_Follows_FollowerId");

            migrationBuilder.AddColumn<string>(
                name: "FollowedUserId",
                table: "Follows",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_FollowedUserId",
                table: "Follows",
                column: "FollowedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_AspNetUsers_FollowedUserId",
                table: "Follows",
                column: "FollowedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_AspNetUsers_FollowerId",
                table: "Follows",
                column: "FollowerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Follows_AspNetUsers_FollowedUserId",
                table: "Follows");

            migrationBuilder.DropForeignKey(
                name: "FK_Follows_AspNetUsers_FollowerId",
                table: "Follows");

            migrationBuilder.DropIndex(
                name: "IX_Follows_FollowedUserId",
                table: "Follows");

            migrationBuilder.DropColumn(
                name: "FollowedUserId",
                table: "Follows");

            migrationBuilder.RenameColumn(
                name: "FollowerId",
                table: "Follows",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Follows_FollowerId",
                table: "Follows",
                newName: "IX_Follows_UserId");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Follows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Follows_EventId",
                table: "Follows",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_AspNetUsers_UserId",
                table: "Follows",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_Events_EventId",
                table: "Follows",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
