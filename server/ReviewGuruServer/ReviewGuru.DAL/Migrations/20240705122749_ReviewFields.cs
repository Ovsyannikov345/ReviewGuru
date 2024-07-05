using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReviewGuru.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ReviewFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfDeleting",
                table: "Reviews",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfLastModification",
                table: "Reviews",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfDeleting",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "DateOfLastModification",
                table: "Reviews");
        }
    }
}
