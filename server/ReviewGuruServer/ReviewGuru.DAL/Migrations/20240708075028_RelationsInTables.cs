using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReviewGuru.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RelationsInTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MediaId",
                table: "Reviews",
                column: "MediaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaAuthor_AuthorId",
                table: "MediaAuthor",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaAuthor_MediaId",
                table: "MediaAuthor",
                column: "MediaId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaAuthor_Authors_AuthorId",
                table: "MediaAuthor",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaAuthor_Media_MediaId",
                table: "MediaAuthor",
                column: "MediaId",
                principalTable: "Media",
                principalColumn: "MediaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Media_MediaId",
                table: "Reviews",
                column: "MediaId",
                principalTable: "Media",
                principalColumn: "MediaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaAuthor_Authors_AuthorId",
                table: "MediaAuthor");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaAuthor_Media_MediaId",
                table: "MediaAuthor");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Media_MediaId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_MediaId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_MediaAuthor_AuthorId",
                table: "MediaAuthor");

            migrationBuilder.DropIndex(
                name: "IX_MediaAuthor_MediaId",
                table: "MediaAuthor");
        }
    }
}
