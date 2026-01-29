using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbsoluteCinema.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TinyFixMovieDurationConversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1) rename table (Postgres-safe)
            migrationBuilder.Sql(@"ALTER TABLE ""Halls"" RENAME TO halls;");

            // 2) rename primary key constraint (optional but nice)
            migrationBuilder.Sql(@"ALTER TABLE halls RENAME CONSTRAINT ""PK_Halls"" TO ""PK_halls"";");

            // 3) add column to sessions (залишаємо як було)
            migrationBuilder.AddColumn<byte>(
                name: "format",
                table: "sessions",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "format",
                table: "sessions");

            migrationBuilder.Sql(@"ALTER TABLE halls RENAME CONSTRAINT ""PK_halls"" TO ""PK_Halls"";");
            migrationBuilder.Sql(@"ALTER TABLE halls RENAME TO ""Halls"";");
        }
    }
}
