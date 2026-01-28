using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbsoluteCinema.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTypePriceSessionRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SeatTypeId1",
                table: "type_prices",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_type_prices_SeatTypeId1",
                table: "type_prices",
                column: "SeatTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_type_prices_seat_types_SeatTypeId1",
                table: "type_prices",
                column: "SeatTypeId1",
                principalTable: "seat_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_type_prices_seat_types_SeatTypeId1",
                table: "type_prices");

            migrationBuilder.DropIndex(
                name: "IX_type_prices_SeatTypeId1",
                table: "type_prices");

            migrationBuilder.DropColumn(
                name: "SeatTypeId1",
                table: "type_prices");
        }
    }
}
