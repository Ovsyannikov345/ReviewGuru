using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReviewGuru.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationBetweenUsersAndMediaAsFavorites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MediaUser",
                columns: table => new
                {
                    FavoritesMediaId = table.Column<int>(type: "integer", nullable: false),
                    UsersUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaUser", x => new { x.FavoritesMediaId, x.UsersUserId });
                    table.ForeignKey(
                        name: "FK_MediaUser_Media_FavoritesMediaId",
                        column: x => x.FavoritesMediaId,
                        principalTable: "Media",
                        principalColumn: "MediaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MediaUser_Users_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaUser_UsersUserId",
                table: "MediaUser",
                column: "UsersUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaUser");
        }
    }
}
