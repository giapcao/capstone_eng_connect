using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngConnect.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class update_relationship_of_course_module_session_resource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_module_course",
                table: "course_module");

            migrationBuilder.DropForeignKey(
                name: "fk_resource_session",
                table: "course_resource");

            migrationBuilder.DropForeignKey(
                name: "fk_session_module",
                table: "course_session");

            migrationBuilder.DropForeignKey(
                name: "fk_lesson_tutor",
                table: "lesson");

            migrationBuilder.DropIndex(
                name: "IX_lesson_tutor_id",
                table: "lesson");

            migrationBuilder.DropIndex(
                name: "IX_course_session_module_id",
                table: "course_session");

            migrationBuilder.DropIndex(
                name: "IX_course_resource_session_id",
                table: "course_resource");

            migrationBuilder.DropIndex(
                name: "IX_course_module_course_id",
                table: "course_module");

            migrationBuilder.DropColumn(
                name: "tutor_id",
                table: "lesson");

            migrationBuilder.DropColumn(
                name: "module_id",
                table: "course_session");

            migrationBuilder.DropColumn(
                name: "session_number",
                table: "course_session");

            migrationBuilder.DropColumn(
                name: "session_id",
                table: "course_resource");

            migrationBuilder.DropColumn(
                name: "course_id",
                table: "course_module");

            migrationBuilder.DropColumn(
                name: "module_number",
                table: "course_module");

            migrationBuilder.AddColumn<Guid>(
                name: "tutor_id",
                table: "course_session",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "tutor_id",
                table: "course_resource",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "tutor_id",
                table: "course_module",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "course_course_module",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_module_id = table.Column<Guid>(type: "uuid", nullable: false),
                    module_number = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("course_course_module_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_cc_course",
                        column: x => x.course_id,
                        principalTable: "course",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_cc_course_module",
                        column: x => x.course_module_id,
                        principalTable: "course_module",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "course_module_course_session",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_module_id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_session_id = table.Column<Guid>(type: "uuid", nullable: false),
                    session_number = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("course_module_course_session_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_cc_course_module",
                        column: x => x.course_module_id,
                        principalTable: "course_module",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_cc_course_session",
                        column: x => x.course_session_id,
                        principalTable: "course_session",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "course_session_course_resource",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_session_id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_resource_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("course_session_course_resource_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_cc_course_resource",
                        column: x => x.course_resource_id,
                        principalTable: "course_resource",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_cc_course_session",
                        column: x => x.course_session_id,
                        principalTable: "course_session",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_course_session_tutor_id",
                table: "course_session",
                column: "tutor_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_resource_tutor_id",
                table: "course_resource",
                column: "tutor_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_module_tutor_id",
                table: "course_module",
                column: "tutor_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_course_module_course_module_id",
                table: "course_course_module",
                column: "course_module_id");

            migrationBuilder.CreateIndex(
                name: "uq_course_course_module",
                table: "course_course_module",
                columns: new[] { "course_id", "course_module_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_course_module_course_session_course_session_id",
                table: "course_module_course_session",
                column: "course_session_id");

            migrationBuilder.CreateIndex(
                name: "uq_course_module_course_session",
                table: "course_module_course_session",
                columns: new[] { "course_module_id", "course_session_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_course_session_course_resource_course_resource_id",
                table: "course_session_course_resource",
                column: "course_resource_id");

            migrationBuilder.CreateIndex(
                name: "uq_course_session_course_resource",
                table: "course_session_course_resource",
                columns: new[] { "course_session_id", "course_resource_id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_module_tutor",
                table: "course_module",
                column: "tutor_id",
                principalTable: "tutor",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_resource_tutor",
                table: "course_resource",
                column: "tutor_id",
                principalTable: "tutor",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_session_tutor",
                table: "course_session",
                column: "tutor_id",
                principalTable: "tutor",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_cc_course",
                table: "course_category");

            migrationBuilder.DropForeignKey(
                name: "fk_module_tutor",
                table: "course_module");

            migrationBuilder.DropForeignKey(
                name: "fk_resource_tutor",
                table: "course_resource");

            migrationBuilder.DropForeignKey(
                name: "fk_session_tutor",
                table: "course_session");

            migrationBuilder.DropTable(
                name: "course_course_module");

            migrationBuilder.DropTable(
                name: "course_module_course_session");

            migrationBuilder.DropTable(
                name: "course_session_course_resource");

            migrationBuilder.DropIndex(
                name: "IX_course_session_tutor_id",
                table: "course_session");

            migrationBuilder.DropIndex(
                name: "IX_course_resource_tutor_id",
                table: "course_resource");

            migrationBuilder.DropIndex(
                name: "IX_course_module_tutor_id",
                table: "course_module");

            migrationBuilder.DropColumn(
                name: "tutor_id",
                table: "course_session");

            migrationBuilder.DropColumn(
                name: "tutor_id",
                table: "course_resource");

            migrationBuilder.DropColumn(
                name: "tutor_id",
                table: "course_module");

            migrationBuilder.AddColumn<Guid>(
                name: "tutor_id",
                table: "lesson",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "module_id",
                table: "course_session",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "session_number",
                table: "course_session",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "session_id",
                table: "course_resource",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "course_id",
                table: "course_module",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "module_number",
                table: "course_module",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_lesson_tutor_id",
                table: "lesson",
                column: "tutor_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_session_module_id",
                table: "course_session",
                column: "module_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_resource_session_id",
                table: "course_resource",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_module_course_id",
                table: "course_module",
                column: "course_id");

            migrationBuilder.AddForeignKey(
                name: "fk_module_course",
                table: "course_module",
                column: "course_id",
                principalTable: "course",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_resource_session",
                table: "course_resource",
                column: "session_id",
                principalTable: "course_session",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_session_module",
                table: "course_session",
                column: "module_id",
                principalTable: "course_module",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_lesson_tutor",
                table: "lesson",
                column: "tutor_id",
                principalTable: "tutor",
                principalColumn: "id");
        }
    }
}
