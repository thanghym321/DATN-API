using Microsoft.EntityFrameworkCore.Migrations;

namespace DATN.DataContextCF.Migrations
{
    public partial class updatefkroommeterreading : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MeterReading_RoomId",
                table: "MeterReading");

            migrationBuilder.CreateIndex(
                name: "IX_MeterReading_RoomId",
                table: "MeterReading",
                column: "RoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MeterReading_RoomId",
                table: "MeterReading");

            migrationBuilder.CreateIndex(
                name: "IX_MeterReading_RoomId",
                table: "MeterReading",
                column: "RoomId",
                unique: true);
        }
    }
}
