using Microsoft.EntityFrameworkCore.Migrations;

namespace DATN.DataContextCF.Migrations
{
    public partial class updatestatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "ServiceRegistration",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true,
                oldDefaultValue: "Đang xét duyệt");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "RoomRegistration",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true,
                oldDefaultValue: "Đang xét duyệt");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true,
                oldDefaultValue: "Còn trống");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Payment",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true,
                oldDefaultValue: "Chưa thanh toán");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Account",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true,
                oldDefaultValue: "Đang hoạt động");

            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Account",
                type: "int",
                nullable: false,
                defaultValue: 2,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true,
                oldDefaultValue: "Student");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ServiceRegistration",
                type: "nvarchar(50)",
                nullable: true,
                defaultValue: "Đang xét duyệt",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "RoomRegistration",
                type: "nvarchar(50)",
                nullable: true,
                defaultValue: "Đang xét duyệt",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Room",
                type: "nvarchar(50)",
                nullable: true,
                defaultValue: "Còn trống",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Payment",
                type: "nvarchar(50)",
                nullable: true,
                defaultValue: "Chưa thanh toán",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Account",
                type: "nvarchar(50)",
                nullable: true,
                defaultValue: "Đang hoạt động",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Account",
                type: "nvarchar(50)",
                nullable: true,
                defaultValue: "Student",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 2);
        }
    }
}
