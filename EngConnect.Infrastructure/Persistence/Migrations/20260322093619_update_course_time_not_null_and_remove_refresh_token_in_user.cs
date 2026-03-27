using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngConnect.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class update_course_time_not_null_and_remove_refresh_token_in_user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "refresh_token",
                table: "user");

            migrationBuilder.AlterColumn<int>(
                name: "nums_session_in_week",
                table: "course",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "number_of_sessions",
                table: "course",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "refresh_token",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "nums_session_in_week",
                table: "course",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "number_of_sessions",
                table: "course",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
