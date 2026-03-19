using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngConnect.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSchedules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "community_comment");

            migrationBuilder.DropTable(
                name: "community_post");

            migrationBuilder.CreateTable(
                name: "enrollment_slot",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    enrollment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tutor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    weekday = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    start_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    end_time = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("enrollment_slot_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_es_enrollment",
                        column: x => x.enrollment_id,
                        principalTable: "course_enrollment",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_es_tutor",
                        column: x => x.tutor_id,
                        principalTable: "tutor",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "lesson_reschedule_request",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    lesson_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    proposed_start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    proposed_end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValueSql: "'pending'::character varying"),
                    tutor_note = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("lesson_reschedule_request_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_lrr_lesson",
                        column: x => x.lesson_id,
                        principalTable: "lesson",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_lrr_student",
                        column: x => x.student_id,
                        principalTable: "student",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_enrollment_slot_enrollment_id",
                table: "enrollment_slot",
                column: "enrollment_id");

            migrationBuilder.CreateIndex(
                name: "uq_locked_slot",
                table: "enrollment_slot",
                columns: new[] { "tutor_id", "weekday", "start_time", "end_time" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_lesson_reschedule_request_lesson_id",
                table: "lesson_reschedule_request",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_reschedule_request_student_id",
                table: "lesson_reschedule_request",
                column: "student_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "enrollment_slot");

            migrationBuilder.DropTable(
                name: "lesson_reschedule_request");

            migrationBuilder.CreateTable(
                name: "community_comment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    author_id = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    parent_comment_id = table.Column<Guid>(type: "uuid", nullable: true),
                    post_id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("community_comment_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "community_post",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    author_id = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_pinned = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("community_post_pkey", x => x.id);
                });
        }
    }
}
