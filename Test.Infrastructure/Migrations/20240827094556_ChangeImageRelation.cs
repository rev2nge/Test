using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Test.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeImageRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Announcements");

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementImages_AnnouncementId",
                table: "AnnouncementImages",
                column: "AnnouncementId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnnouncementImages_Announcements_AnnouncementId",
                table: "AnnouncementImages",
                column: "AnnouncementId",
                principalTable: "Announcements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnnouncementImages_Announcements_AnnouncementId",
                table: "AnnouncementImages");

            migrationBuilder.DropIndex(
                name: "IX_AnnouncementImages_AnnouncementId",
                table: "AnnouncementImages");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Announcements",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
