using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ReviewGuru.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveexplicitmanyToManytable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaAuthor");

            migrationBuilder.CreateTable(
                name: "AuthorMedia",
                columns: table => new
                {
                    AuthorsAuthorId = table.Column<int>(type: "integer", nullable: false),
                    MediaId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorMedia", x => new { x.AuthorsAuthorId, x.MediaId });
                    table.ForeignKey(
                        name: "FK_AuthorMedia_Authors_AuthorsAuthorId",
                        column: x => x.AuthorsAuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorMedia_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Media",
                        principalColumn: "MediaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorMedia_MediaId",
                table: "AuthorMedia",
                column: "MediaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorMedia");

            migrationBuilder.CreateTable(
                name: "MediaAuthor",
                columns: table => new
                {
                    MediaAuthorId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuthorId = table.Column<int>(type: "integer", nullable: true),
                    MediaId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaAuthor", x => x.MediaAuthorId);
                    table.ForeignKey(
                        name: "FK_MediaAuthor_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId");
                    table.ForeignKey(
                        name: "FK_MediaAuthor_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Media",
                        principalColumn: "MediaId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaAuthor_AuthorId",
                table: "MediaAuthor",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaAuthor_MediaId",
                table: "MediaAuthor",
                column: "MediaId");
        }
    }
}
