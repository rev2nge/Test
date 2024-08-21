using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Test.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "Announcements");

            migrationBuilder.CreateTable(
                name: "AnnouncementImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnnouncementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ThumbnailImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ImageFormat = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnnouncementImages_Announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "Announcements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementImages_AnnouncementId",
                table: "AnnouncementImages",
                column: "AnnouncementId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnouncementImages");

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "Announcements",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
