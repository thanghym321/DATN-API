using Microsoft.EntityFrameworkCore.Migrations;

namespace DATN.DataContextCF.Migrations
{
    public partial class updatetablefeedbackreport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Report",
                type: "nvarchar(250)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "Feedback",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Feedback",
                type: "nvarchar(250)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_ReportId",
                table: "Feedback",
                column: "ReportId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_Report_ReportId",
                table: "Feedback",
                column: "ReportId",
                principalTable: "Report",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Report_ReportId",
                table: "Feedback");

            migrationBuilder.DropIndex(
                name: "IX_Feedback_ReportId",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Feedback");
        }
    }
}
