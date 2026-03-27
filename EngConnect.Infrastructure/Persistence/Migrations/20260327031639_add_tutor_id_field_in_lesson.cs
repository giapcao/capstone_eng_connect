using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngConnect.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class add_tutor_id_field_in_lesson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "quiz_attempt");

            migrationBuilder.DropTable(
                name: "quiz_attempt_answer");

            migrationBuilder.DropTable(
                name: "quiz_question");

            migrationBuilder.DropTable(
                name: "quiz");

            migrationBuilder.AddColumn<Guid>(
                name: "tutor_id",
                table: "lesson",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_lesson_tutor_id",
                table: "lesson",
                column: "tutor_id");

            migrationBuilder.AddForeignKey(
                name: "fk_lesson_tutor",
                table: "lesson",
                column: "tutor_id",
                principalTable: "tutor",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_lesson_tutor",
                table: "lesson");

            migrationBuilder.DropIndex(
                name: "IX_lesson_tutor_id",
                table: "lesson");

            migrationBuilder.DropColumn(
                name: "tutor_id",
                table: "lesson");

            migrationBuilder.CreateTable(
                name: "quiz",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_id = table.Column<Guid>(type: "uuid", nullable: false),
                    attempt_limit = table.Column<int>(type: "integer", nullable: true, defaultValue: 1),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    duration_seconds = table.Column<int>(type: "integer", nullable: false),
                    expired_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_open = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    max_score = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("quiz_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_quiz_course",
                        column: x => x.course_id,
                        principalTable: "course",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "quiz_attempt",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    duration_seconds = table.Column<int>(type: "integer", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    quiz_id = table.Column<Guid>(type: "uuid", nullable: false),
                    score = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("quiz_attempt_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "quiz_attempt_answer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    answer = table.Column<string>(type: "text", nullable: true),
                    attempt_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_correct = table.Column<bool>(type: "boolean", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    question_id = table.Column<Guid>(type: "uuid", nullable: false),
                    receive_point = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("quiz_attempt_answer_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "quiz_question",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    quiz_id = table.Column<Guid>(type: "uuid", nullable: false),
                    correct_answer = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    options = table.Column<string>(type: "json", nullable: true),
                    point = table.Column<int>(type: "integer", nullable: false),
                    question_number = table.Column<int>(type: "integer", nullable: false),
                    question_text = table.Column<string>(type: "text", nullable: false),
                    question_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("quiz_question_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_quiz_quizquestion",
                        column: x => x.quiz_id,
                        principalTable: "quiz",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_quiz_course_id",
                table: "quiz",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_quiz_question_quiz_id",
                table: "quiz_question",
                column: "quiz_id");
        }
    }
}
