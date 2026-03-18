using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EngConnect.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pgcrypto", ",,")
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("category_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "commission_config",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    commission_percent = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    apply_from = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    apply_to = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("commission_config_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "email_template",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    subject = table.Column<string>(type: "text", nullable: false),
                    body = table.Column<string>(type: "text", nullable: false),
                    event_type = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("email_template_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "outbox_event",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    aggregate_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    aggregate_id = table.Column<Guid>(type: "uuid", nullable: false),
                    event_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    event_data = table.Column<string>(type: "text", nullable: false),
                    outbox_status = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    processed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    sent_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    failed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    dead_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    lock_by = table.Column<Guid>(type: "uuid", maxLength: 100, nullable: true),
                    lock_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    retry_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    next_retry_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("outbox_event_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permission",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("permission_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "quiz_attempt",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    quiz_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    score = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    duration_seconds = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                    attempt_id = table.Column<Guid>(type: "uuid", nullable: false),
                    question_id = table.Column<Guid>(type: "uuid", nullable: false),
                    answer = table.Column<string>(type: "text", nullable: true),
                    is_correct = table.Column<bool>(type: "boolean", nullable: true),
                    receive_point = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("quiz_attempt_answer_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("role_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "schedule_job_tracking",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    job_name = table.Column<string>(type: "text", nullable: false),
                    execute_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    last_fire_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    next_fire_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    run_count = table.Column<int>(type: "integer", nullable: false),
                    is_executed = table.Column<bool>(type: "boolean", nullable: true),
                    last_fire_failed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    last_fire_succeeded_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    job_type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedule_job_tracking", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    user_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    address_num = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    province_id = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    province_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ward_id = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    ward_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    refresh_token = table.Column<string>(type: "text", nullable: true),
                    is_email_verified = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permission_role",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    permission_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("permission_role_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_permission",
                        column: x => x.permission_id,
                        principalTable: "permission",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_role",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    commission = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    payment_reference = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    meta_data = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("order_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_student",
                        column: x => x.student_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "student",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    notes = table.Column<string>(type: "text", nullable: true),
                    school = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    grade = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    @class = table.Column<string>(name: "class", type: "character varying(50)", maxLength: 50, nullable: true),
                    tags = table.Column<List<string>>(type: "text[]", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    avatar = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("student_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_student_user",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "support_ticket",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_by = table.Column<Guid>(type: "uuid", nullable: false),
                    subject = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    priority = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    closed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("support_ticket_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_ticket_created_by",
                        column: x => x.created_by,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tutor",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    headline = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    bio = table.Column<string>(type: "text", nullable: true),
                    intro_video_url = table.Column<string>(type: "text", nullable: true),
                    years_experience = table.Column<int>(type: "integer", nullable: true),
                    cv_url = table.Column<string>(type: "text", nullable: true),
                    tags = table.Column<List<string>>(type: "text[]", nullable: true),
                    slots_count = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    verified_status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    rating_average = table.Column<decimal>(type: "numeric(3,2)", precision: 3, scale: 2, nullable: true, defaultValueSql: "0"),
                    rating_count = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tutor_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_tutor_user",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user_oauth_account",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    provider = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    provider_user_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_oauth_account_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_oauth_user",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user_role",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_role_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_role",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_user",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "payment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, defaultValueSql: "'pending'::character varying"),
                    amount = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    bank_transaction_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("payment_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_payment_order",
                        column: x => x.order_id,
                        principalTable: "order",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "support_ticket_message",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    ticket_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sender_id = table.Column<Guid>(type: "uuid", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("support_ticket_message_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_ticket_message_sender",
                        column: x => x.sender_id,
                        principalTable: "user",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_ticket_message_ticket",
                        column: x => x.ticket_id,
                        principalTable: "support_ticket",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "conversation",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tutor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("conversation_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_conversation_student",
                        column: x => x.student_id,
                        principalTable: "student",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_conversation_tutor",
                        column: x => x.tutor_id,
                        principalTable: "tutor",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "course",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tutor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    parent_course_id = table.Column<Guid>(type: "uuid", nullable: true),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    short_description = table.Column<string>(type: "text", nullable: true),
                    full_description = table.Column<string>(type: "text", nullable: true),
                    outcomes = table.Column<string>(type: "text", nullable: true),
                    level = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    estimated_time = table.Column<TimeSpan>(type: "interval", nullable: true),
                    estimated_time_lesson = table.Column<TimeSpan>(type: "interval", nullable: true),
                    price = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    number_of_sessions = table.Column<int>(type: "integer", nullable: true),
                    nums_session_in_week = table.Column<int>(type: "integer", nullable: true),
                    thumbnail_url = table.Column<string>(type: "text", nullable: true),
                    demo_video_url = table.Column<string>(type: "text", nullable: true),
                    number_of_enrollment = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    rating_average = table.Column<decimal>(type: "numeric(3,2)", precision: 3, scale: 2, nullable: true, defaultValueSql: "0"),
                    rating_count = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    is_certificate = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("course_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_course_parent",
                        column: x => x.parent_course_id,
                        principalTable: "course",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_course_tutor",
                        column: x => x.tutor_id,
                        principalTable: "tutor",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tutor_document",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tutor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    doc_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    url = table.Column<string>(type: "text", nullable: false),
                    issued_by = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    issued_at = table.Column<DateOnly>(type: "date", nullable: true),
                    expired_at = table.Column<DateOnly>(type: "date", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tutor_document_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_document_tutor",
                        column: x => x.tutor_id,
                        principalTable: "tutor",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tutor_schedule",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    table.PrimaryKey("tutor_schedule_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_schedule_tutor",
                        column: x => x.tutor_id,
                        principalTable: "tutor",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "tutor_verification_request",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tutor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    submitted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    reviewed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    reviewed_by = table.Column<Guid>(type: "uuid", nullable: true),
                    rejection_reason = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tutor_verification_request_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_reviewed_by",
                        column: x => x.reviewed_by,
                        principalTable: "user",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_verification_tutor",
                        column: x => x.tutor_id,
                        principalTable: "tutor",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "conversation_message",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    conversation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sender_id = table.Column<Guid>(type: "uuid", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("conversation_message_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_conversation_message",
                        column: x => x.conversation_id,
                        principalTable: "conversation",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "actual_schedule",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tutor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: true),
                    course_id = table.Column<Guid>(type: "uuid", nullable: true),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("actual_schedule_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_actual_course",
                        column: x => x.course_id,
                        principalTable: "course",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_actual_student",
                        column: x => x.student_id,
                        principalTable: "student",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_actual_tutor",
                        column: x => x.tutor_id,
                        principalTable: "tutor",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "course_category",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_id = table.Column<Guid>(type: "uuid", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("course_category_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_cc_category",
                        column: x => x.category_id,
                        principalTable: "category",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_cc_course",
                        column: x => x.course_id,
                        principalTable: "course",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "course_enrollment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    price_at_purchase = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: true),
                    currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    nums_of_session = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    enrolled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    expired_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("course_enrollment_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_enrollment_course",
                        column: x => x.course_id,
                        principalTable: "course",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_enrollment_student",
                        column: x => x.student_id,
                        principalTable: "student",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "course_module",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    outcomes = table.Column<string>(type: "text", nullable: true),
                    module_number = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("course_module_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_module_course",
                        column: x => x.course_id,
                        principalTable: "course",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "course_verification_request",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    submitted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    reviewed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    reviewed_by = table.Column<Guid>(type: "uuid", nullable: true),
                    rejection_reason = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("course_verification_request_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_verification_course",
                        column: x => x.course_id,
                        principalTable: "course",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_verification_reviewer",
                        column: x => x.reviewed_by,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "order_detail",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_id = table.Column<Guid>(type: "uuid", nullable: false),
                    price_at_purchase = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    meta_data = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("order_detail_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_detail_course",
                        column: x => x.course_id,
                        principalTable: "course",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_order_detail_order",
                        column: x => x.order_id,
                        principalTable: "order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "quiz",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    is_open = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    max_score = table.Column<int>(type: "integer", nullable: false),
                    duration_seconds = table.Column<int>(type: "integer", nullable: false),
                    attempt_limit = table.Column<int>(type: "integer", nullable: true, defaultValue: 1),
                    expired_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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
                name: "course_review",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    course_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tutor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    enrollment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rating = table.Column<short>(type: "smallint", nullable: true),
                    comment = table.Column<string>(type: "text", nullable: true),
                    is_anonymous = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("course_review_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_review_course",
                        column: x => x.course_id,
                        principalTable: "course",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_review_enrollment",
                        column: x => x.enrollment_id,
                        principalTable: "course_enrollment",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_review_student",
                        column: x => x.student_id,
                        principalTable: "student",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_review_tutor",
                        column: x => x.tutor_id,
                        principalTable: "tutor",
                        principalColumn: "id");
                });

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
                name: "course_session",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    module_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    outcomes = table.Column<string>(type: "text", nullable: true),
                    session_number = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("course_session_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_session_module",
                        column: x => x.module_id,
                        principalTable: "course_module",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "quiz_question",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    quiz_id = table.Column<Guid>(type: "uuid", nullable: false),
                    question_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    question_number = table.Column<int>(type: "integer", nullable: false),
                    question_text = table.Column<string>(type: "text", nullable: false),
                    options = table.Column<string>(type: "json", nullable: true),
                    correct_answer = table.Column<string>(type: "text", nullable: true),
                    point = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "course_resource",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    session_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    resource_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    url = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("course_resource_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_resource_session",
                        column: x => x.session_id,
                        principalTable: "course_session",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "lesson",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tutor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    enrollment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    session_id = table.Column<Guid>(type: "uuid", nullable: true),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    meeting_url = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("lesson_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_lesson_enrollment",
                        column: x => x.enrollment_id,
                        principalTable: "course_enrollment",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_lesson_session",
                        column: x => x.session_id,
                        principalTable: "course_session",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_lesson_student",
                        column: x => x.student_id,
                        principalTable: "student",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_lesson_tutor",
                        column: x => x.tutor_id,
                        principalTable: "tutor",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "lesson_homework",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    lesson_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    resource_url = table.Column<string>(type: "text", nullable: true),
                    submission_url = table.Column<string>(type: "text", nullable: true),
                    score = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    max_score = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    tutor_feedback = table.Column<string>(type: "text", nullable: true),
                    assigned_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    submitted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    due_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false, defaultValueSql: "'not_started'::character varying"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("lesson_homework_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_hw_lesson",
                        column: x => x.lesson_id,
                        principalTable: "lesson",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "lesson_record",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    lesson_id = table.Column<Guid>(type: "uuid", nullable: false),
                    record_url = table.Column<string>(type: "text", nullable: false),
                    duration_seconds = table.Column<int>(type: "integer", nullable: true),
                    recording_started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    recording_ended_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("lesson_record_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_record_lesson",
                        column: x => x.lesson_id,
                        principalTable: "lesson",
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

            migrationBuilder.CreateTable(
                name: "lesson_script",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    lesson_id = table.Column<Guid>(type: "uuid", nullable: false),
                    record_id = table.Column<Guid>(type: "uuid", nullable: false),
                    language = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    full_text = table.Column<string>(type: "text", nullable: true),
                    summarize_text = table.Column<string>(type: "text", nullable: true),
                    lesson_outcome = table.Column<string>(type: "text", nullable: true),
                    coverage_percent = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "now()"),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("lesson_script_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_script_lesson",
                        column: x => x.lesson_id,
                        principalTable: "lesson",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_script_record",
                        column: x => x.record_id,
                        principalTable: "lesson_record",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_actual_schedule_course_id",
                table: "actual_schedule",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_actual_schedule_student_id",
                table: "actual_schedule",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_actual_schedule_tutor_id",
                table: "actual_schedule",
                column: "tutor_id");

            migrationBuilder.CreateIndex(
                name: "IX_conversation_student_id",
                table: "conversation",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "uq_conversation_tutor_student",
                table: "conversation",
                columns: new[] { "tutor_id", "student_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_conversation_message_conversation_id",
                table: "conversation_message",
                column: "conversation_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_parent_course_id",
                table: "course",
                column: "parent_course_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_tutor_id",
                table: "course",
                column: "tutor_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_category_category_id",
                table: "course_category",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "uq_course_category",
                table: "course_category",
                columns: new[] { "course_id", "category_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_course_enrollment_student_id",
                table: "course_enrollment",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "uq_course_student",
                table: "course_enrollment",
                columns: new[] { "course_id", "student_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_course_module_course_id",
                table: "course_module",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_resource_session_id",
                table: "course_resource",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_review_course_id",
                table: "course_review",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_review_student_id",
                table: "course_review",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_review_tutor_id",
                table: "course_review",
                column: "tutor_id");

            migrationBuilder.CreateIndex(
                name: "uq_review_enrollment",
                table: "course_review",
                column: "enrollment_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_course_session_module_id",
                table: "course_session",
                column: "module_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_verification_request_course_id",
                table: "course_verification_request",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_verification_request_reviewed_by",
                table: "course_verification_request",
                column: "reviewed_by");

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
                name: "IX_lesson_enrollment_id",
                table: "lesson",
                column: "enrollment_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_session_id",
                table: "lesson",
                column: "session_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_student_id",
                table: "lesson",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_tutor_id",
                table: "lesson",
                column: "tutor_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_homework_lesson_id",
                table: "lesson_homework",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_record_lesson_id",
                table: "lesson_record",
                column: "lesson_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_lesson_reschedule_request_lesson_id",
                table: "lesson_reschedule_request",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_reschedule_request_student_id",
                table: "lesson_reschedule_request",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_script_lesson_id",
                table: "lesson_script",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_script_record_id",
                table: "lesson_script",
                column: "record_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_student_id",
                table: "order",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_detail_course_id",
                table: "order_detail",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_detail_order_id",
                table: "order_detail",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_payment_order_id",
                table: "payment",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "permission_code_key",
                table: "permission",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_permission_role_role_id",
                table: "permission_role",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "uq_permission_role_permission_role",
                table: "permission_role",
                columns: new[] { "permission_id", "role_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_quiz_course_id",
                table: "quiz",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_quiz_question_quiz_id",
                table: "quiz_question",
                column: "quiz_id");

            migrationBuilder.CreateIndex(
                name: "role_code_key",
                table: "role",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "uq_student_user",
                table: "student",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_support_ticket_created_by",
                table: "support_ticket",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_support_ticket_message_sender_id",
                table: "support_ticket_message",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "IX_support_ticket_message_ticket_id",
                table: "support_ticket_message",
                column: "ticket_id");

            migrationBuilder.CreateIndex(
                name: "uq_tutor_user",
                table: "tutor",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tutor_document_tutor_id",
                table: "tutor_document",
                column: "tutor_id");

            migrationBuilder.CreateIndex(
                name: "IX_tutor_schedule_tutor_id",
                table: "tutor_schedule",
                column: "tutor_id");

            migrationBuilder.CreateIndex(
                name: "IX_tutor_verification_request_reviewed_by",
                table: "tutor_verification_request",
                column: "reviewed_by");

            migrationBuilder.CreateIndex(
                name: "IX_tutor_verification_request_tutor_id",
                table: "tutor_verification_request",
                column: "tutor_id");

            migrationBuilder.CreateIndex(
                name: "user_email_key",
                table: "user",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_oauth_account_user_id",
                table: "user_oauth_account",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "uq_provider_user",
                table: "user_oauth_account",
                columns: new[] { "provider", "provider_user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_role_role_id",
                table: "user_role",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "uq_user_role_user_role",
                table: "user_role",
                columns: new[] { "user_id", "role_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "actual_schedule");

            migrationBuilder.DropTable(
                name: "commission_config");

            migrationBuilder.DropTable(
                name: "conversation_message");

            migrationBuilder.DropTable(
                name: "course_category");

            migrationBuilder.DropTable(
                name: "course_resource");

            migrationBuilder.DropTable(
                name: "course_review");

            migrationBuilder.DropTable(
                name: "course_verification_request");

            migrationBuilder.DropTable(
                name: "email_template");

            migrationBuilder.DropTable(
                name: "enrollment_slot");

            migrationBuilder.DropTable(
                name: "lesson_homework");

            migrationBuilder.DropTable(
                name: "lesson_reschedule_request");

            migrationBuilder.DropTable(
                name: "lesson_script");

            migrationBuilder.DropTable(
                name: "order_detail");

            migrationBuilder.DropTable(
                name: "outbox_event");

            migrationBuilder.DropTable(
                name: "payment");

            migrationBuilder.DropTable(
                name: "permission_role");

            migrationBuilder.DropTable(
                name: "quiz_attempt");

            migrationBuilder.DropTable(
                name: "quiz_attempt_answer");

            migrationBuilder.DropTable(
                name: "quiz_question");

            migrationBuilder.DropTable(
                name: "schedule_job_tracking");

            migrationBuilder.DropTable(
                name: "support_ticket_message");

            migrationBuilder.DropTable(
                name: "tutor_document");

            migrationBuilder.DropTable(
                name: "tutor_schedule");

            migrationBuilder.DropTable(
                name: "tutor_verification_request");

            migrationBuilder.DropTable(
                name: "user_oauth_account");

            migrationBuilder.DropTable(
                name: "user_role");

            migrationBuilder.DropTable(
                name: "conversation");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "lesson_record");

            migrationBuilder.DropTable(
                name: "order");

            migrationBuilder.DropTable(
                name: "permission");

            migrationBuilder.DropTable(
                name: "quiz");

            migrationBuilder.DropTable(
                name: "support_ticket");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "lesson");

            migrationBuilder.DropTable(
                name: "course_enrollment");

            migrationBuilder.DropTable(
                name: "course_session");

            migrationBuilder.DropTable(
                name: "student");

            migrationBuilder.DropTable(
                name: "course_module");

            migrationBuilder.DropTable(
                name: "course");

            migrationBuilder.DropTable(
                name: "tutor");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
