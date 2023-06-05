using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DATN.DataContextCF.Migrations
{
    public partial class updatemeterreadingelectricitywaterrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AmountUsed",
                table: "ElectricityWaterRate",
                newName: "Tier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "MeterReading",
                type: "datetime",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AddColumn<int>(
                name: "EndAmount",
                table: "ElectricityWaterRate",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartAmount",
                table: "ElectricityWaterRate",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndAmount",
                table: "ElectricityWaterRate");

            migrationBuilder.DropColumn(
                name: "StartAmount",
                table: "ElectricityWaterRate");

            migrationBuilder.RenameColumn(
                name: "Tier",
                table: "ElectricityWaterRate",
                newName: "AmountUsed");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "MeterReading",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "getdate()");
        }
    }
}
