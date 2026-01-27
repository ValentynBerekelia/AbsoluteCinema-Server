using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbsoluteCinema.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMovieFormatToSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Halls",
                table: "Halls");

            migrationBuilder.RenameTable(
                name: "Halls",
                newName: "halls");

            migrationBuilder.AddColumn<byte>(
                name: "format",
                table: "sessions",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_halls",
                table: "halls",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_halls",
                table: "halls");

            migrationBuilder.DropColumn(
                name: "format",
                table: "sessions");

            migrationBuilder.RenameTable(
                name: "halls",
                newName: "Halls");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Halls",
                table: "Halls",
                column: "id");
        }
    }
}
