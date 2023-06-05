using Microsoft.EntityFrameworkCore.Migrations;

namespace DATN.DataContextCF.Migrations
{
    public partial class addcolumnunit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "ElectricityWaterRate",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Unit",
                table: "ElectricityWaterRate");
        }
    }
}
