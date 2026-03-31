using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngConnect.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class update_relationship_of_lesson_lesson_script : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_lesson_script_lesson_id",
                table: "lesson_script");

            migrationBuilder.DropIndex(
                name: "IX_lesson_script_record_id",
                table: "lesson_script");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_script_lesson_id",
                table: "lesson_script",
                column: "lesson_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_lesson_script_record_id",
                table: "lesson_script",
                column: "record_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_lesson_script_lesson_id",
                table: "lesson_script");

            migrationBuilder.DropIndex(
                name: "IX_lesson_script_record_id",
                table: "lesson_script");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_script_lesson_id",
                table: "lesson_script",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_script_record_id",
                table: "lesson_script",
                column: "record_id");
        }
    }
}
