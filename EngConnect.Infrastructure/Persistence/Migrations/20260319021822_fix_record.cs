using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngConnect.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class fix_record : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_lesson_record_lesson_id",
                table: "lesson_record");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_record_lesson_id",
                table: "lesson_record",
                column: "lesson_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_lesson_record_lesson_id",
                table: "lesson_record");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_record_lesson_id",
                table: "lesson_record",
                column: "lesson_id");
        }
    }
}
