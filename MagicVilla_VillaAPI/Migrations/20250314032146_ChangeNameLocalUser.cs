using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_VillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameLocalUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUsers",
                table: "ApplicationUsers");

            migrationBuilder.RenameTable(
                name: "ApplicationUsers",
                newName: "LocalUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocalUsers",
                table: "LocalUsers",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LocalUsers",
                table: "LocalUsers");

            migrationBuilder.RenameTable(
                name: "LocalUsers",
                newName: "ApplicationUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUsers",
                table: "ApplicationUsers",
                column: "Id");
        }
    }
}
