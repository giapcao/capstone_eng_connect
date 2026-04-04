using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngConnect.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class add_parent_id_in_module_and_session_to_make_versioning : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_lesson_record_lesson_id",
                table: "lesson_record");

            migrationBuilder.AddColumn<Guid>(
                name: "module_id",
                table: "lesson",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "parent_session_id",
                table: "course_session",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "parent_resource_id",
                table: "course_resource",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "parent_module_id",
                table: "course_module",
                type: "uuid",
                nullable: true);
            
            migrationBuilder.CreateIndex(
                name: "IX_lesson_record_lesson_id",
                table: "lesson_record",
                column: "lesson_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_lesson_module_id",
                table: "lesson",
                column: "module_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_session_parent_session_id",
                table: "course_session",
                column: "parent_session_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_resource_parent_resource_id",
                table: "course_resource",
                column: "parent_resource_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_module_parent_module_id",
                table: "course_module",
                column: "parent_module_id");

            migrationBuilder.AddForeignKey(
                name: "fk_course_module_parent",
                table: "course_module",
                column: "parent_module_id",
                principalTable: "course_module",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_course_resource_parent",
                table: "course_resource",
                column: "parent_resource_id",
                principalTable: "course_resource",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_course_session_parent",
                table: "course_session",
                column: "parent_session_id",
                principalTable: "course_session",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_lesson_module",
                table: "lesson",
                column: "module_id",
                principalTable: "course_module",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_course_module_parent",
                table: "course_module");

            migrationBuilder.DropForeignKey(
                name: "fk_course_resource_parent",
                table: "course_resource");

            migrationBuilder.DropForeignKey(
                name: "fk_course_session_parent",
                table: "course_session");

            migrationBuilder.DropForeignKey(
                name: "fk_lesson_module",
                table: "lesson");

            migrationBuilder.DropIndex(
                name: "IX_lesson_record_lesson_id",
                table: "lesson_record");

            migrationBuilder.DropIndex(
                name: "IX_lesson_module_id",
                table: "lesson");

            migrationBuilder.DropIndex(
                name: "IX_course_session_parent_session_id",
                table: "course_session");

            migrationBuilder.DropIndex(
                name: "IX_course_resource_parent_resource_id",
                table: "course_resource");

            migrationBuilder.DropIndex(
                name: "IX_course_module_parent_module_id",
                table: "course_module");
            
            migrationBuilder.DropColumn(
                name: "module_id",
                table: "lesson");

            migrationBuilder.DropColumn(
                name: "parent_session_id",
                table: "course_session");

            migrationBuilder.DropColumn(
                name: "parent_resource_id",
                table: "course_resource");

            migrationBuilder.DropColumn(
                name: "parent_module_id",
                table: "course_module");
            
            migrationBuilder.CreateIndex(
                name: "IX_lesson_record_lesson_id",
                table: "lesson_record",
                column: "lesson_id");
        }
    }
}
