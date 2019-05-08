using Microsoft.EntityFrameworkCore.Migrations;

namespace Course.Data.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Candidate",
                nullable: true,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Candidate",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}
