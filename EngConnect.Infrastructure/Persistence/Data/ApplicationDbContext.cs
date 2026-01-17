using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;
using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EngConnect.Infrastructure.Persistence.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActualSchedule> ActualSchedules { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CommissionConfig> CommissionConfigs { get; set; }

    public virtual DbSet<CommunityComment> CommunityComments { get; set; }

    public virtual DbSet<CommunityPost> CommunityPosts { get; set; }

    public virtual DbSet<Conversation> Conversations { get; set; }

    public virtual DbSet<ConversationMessage> ConversationMessages { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseCategory> CourseCategories { get; set; }

    public virtual DbSet<CourseEnrollment> CourseEnrollments { get; set; }

    public virtual DbSet<CourseModule> CourseModules { get; set; }

    public virtual DbSet<CourseResource> CourseResources { get; set; }

    public virtual DbSet<CourseReview> CourseReviews { get; set; }

    public virtual DbSet<CourseSession> CourseSessions { get; set; }

    public virtual DbSet<CourseVerificationRequest> CourseVerificationRequests { get; set; }

    public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<LessonHomework> LessonHomeworks { get; set; }

    public virtual DbSet<LessonRecord> LessonRecords { get; set; }

    public virtual DbSet<LessonScript> LessonScripts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OutboxEvent> OutboxEvents { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<PermissionRole> PermissionRoles { get; set; }

    public virtual DbSet<Quiz> Quizzes { get; set; }

    public virtual DbSet<QuizAttempt> QuizAttempts { get; set; }

    public virtual DbSet<QuizAttemptAnswer> QuizAttemptAnswers { get; set; }

    public virtual DbSet<QuizQuestion> QuizQuestions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<SupportTicket> SupportTickets { get; set; }

    public virtual DbSet<SupportTicketMessage> SupportTicketMessages { get; set; }

    public virtual DbSet<Tutor> Tutors { get; set; }

    public virtual DbSet<TutorDocument> TutorDocuments { get; set; }

    public virtual DbSet<TutorSchedule> TutorSchedules { get; set; }

    public virtual DbSet<TutorVerificationRequest> TutorVerificationRequests { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserOauthAccount> UserOauthAccounts { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SoftDeleteFilter(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder
            .HasPostgresExtension("pgcrypto")
            .HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<ActualSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("actual_schedule_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Course).WithMany(p => p.ActualSchedules).HasConstraintName("fk_actual_course");

            entity.HasOne(d => d.Student).WithMany(p => p.ActualSchedules).HasConstraintName("fk_actual_student");

            entity.HasOne(d => d.Tutor).WithMany(p => p.ActualSchedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_actual_tutor");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("category_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<CommissionConfig>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("commission_config_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<CommunityComment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("community_comment_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<CommunityPost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("community_post_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsPinned).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("conversation_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Student).WithMany(p => p.Conversations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_conversation_student");

            entity.HasOne(d => d.Tutor).WithMany(p => p.Conversations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_conversation_tutor");
        });

        modelBuilder.Entity<ConversationMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("conversation_message_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Conversation).WithMany(p => p.ConversationMessages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_conversation_message");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("course_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsCertificate).HasDefaultValue(false);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.NumberOfEnrollment).HasDefaultValue(0);
            entity.Property(e => e.RatingAverage).HasDefaultValueSql("0");
            entity.Property(e => e.RatingCount).HasDefaultValue(0);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.ParentCourse).WithMany(p => p.InverseParentCourse).HasConstraintName("fk_course_parent");

            entity.HasOne(d => d.Tutor).WithMany(p => p.Courses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_course_tutor");
        });

        modelBuilder.Entity<CourseCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("course_category_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Category).WithMany(p => p.CourseCategories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_cc_category");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseCategories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_cc_course");
        });

        modelBuilder.Entity<CourseEnrollment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("course_enrollment_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseEnrollments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_enrollment_course");

            entity.HasOne(d => d.Student).WithMany(p => p.CourseEnrollments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_enrollment_student");
        });

        modelBuilder.Entity<CourseModule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("course_module_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseModules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_module_course");
        });

        modelBuilder.Entity<CourseResource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("course_resource_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Session).WithMany(p => p.CourseResources)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_resource_session");
        });

        modelBuilder.Entity<CourseReview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("course_review_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsAnonymous).HasDefaultValue(false);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseReviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_review_course");

            entity.HasOne(d => d.Enrollment).WithOne(p => p.CourseReview)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_review_enrollment");

            entity.HasOne(d => d.Student).WithMany(p => p.CourseReviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_review_student");

            entity.HasOne(d => d.Tutor).WithMany(p => p.CourseReviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_review_tutor");
        });

        modelBuilder.Entity<CourseSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("course_session_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Module).WithMany(p => p.CourseSessions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_session_module");
        });

        modelBuilder.Entity<CourseVerificationRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("course_verification_request_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Course).WithMany(p => p.CourseVerificationRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_verification_course");

            entity.HasOne(d => d.ReviewedByNavigation).WithMany(p => p.CourseVerificationRequests).HasConstraintName("fk_verification_reviewer");
        });

        modelBuilder.Entity<EmailTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("email_template_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.EmailTemplates).HasConstraintName("fk_email_template_updated_by");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("lesson_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Enrollment).WithMany(p => p.Lessons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_lesson_enrollment");

            entity.HasOne(d => d.Session).WithMany(p => p.Lessons).HasConstraintName("fk_lesson_session");

            entity.HasOne(d => d.Student).WithMany(p => p.Lessons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_lesson_student");

            entity.HasOne(d => d.Tutor).WithMany(p => p.Lessons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_lesson_tutor");
        });

        modelBuilder.Entity<LessonHomework>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("lesson_homework_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Status).HasDefaultValueSql("'not_started'::character varying");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Lesson).WithMany(p => p.LessonHomeworks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_hw_lesson");
        });

        modelBuilder.Entity<LessonRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("lesson_record_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Lesson).WithMany(p => p.LessonRecords)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_record_lesson");
        });

        modelBuilder.Entity<LessonScript>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("lesson_script_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Lesson).WithMany(p => p.LessonScripts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_script_lesson");

            entity.HasOne(d => d.Record).WithMany(p => p.LessonScripts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_script_record");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Student).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_student");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_detail_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Course).WithMany(p => p.OrderDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_detail_course");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails).HasConstraintName("fk_order_detail_order");
        });

        modelBuilder.Entity<OutboxEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("outbox_event_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.OutboxStatus).HasDefaultValueSql("'pending'::character varying");
            entity.Property(e => e.RetryCount).HasDefaultValue(0);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("payment_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Status).HasDefaultValueSql("'pending'::character varying");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_payment_order");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("permission_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<PermissionRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("permission_role_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Permission).WithMany(p => p.PermissionRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_permission");

            entity.HasOne(d => d.Role).WithMany(p => p.PermissionRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_role");
        });

        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("quiz_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AttemptLimit).HasDefaultValue(1);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsOpen).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Course).WithMany(p => p.Quizzes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_quiz_course");
        });

        modelBuilder.Entity<QuizAttempt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("quiz_attempt_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<QuizAttemptAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("quiz_attempt_answer_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<QuizQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("quiz_question_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Quiz).WithMany(p => p.QuizQuestions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_quiz_quizquestion");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("role_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("student_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.User).WithOne(p => p.Student)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_student_user");
        });

        modelBuilder.Entity<SupportTicket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("support_ticket_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SupportTickets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ticket_created_by");
        });

        modelBuilder.Entity<SupportTicketMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("support_ticket_message_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Sender).WithMany(p => p.SupportTicketMessages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ticket_message_sender");

            entity.HasOne(d => d.Ticket).WithMany(p => p.SupportTicketMessages).HasConstraintName("fk_ticket_message_ticket");
        });

        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tutor_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.RatingAverage).HasDefaultValueSql("0");
            entity.Property(e => e.RatingCount).HasDefaultValue(0);
            entity.Property(e => e.SlotsCount).HasDefaultValue(0);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.User).WithOne(p => p.Tutor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tutor_user");
        });

        modelBuilder.Entity<TutorDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tutor_document_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Tutor).WithMany(p => p.TutorDocuments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_document_tutor");
        });

        modelBuilder.Entity<TutorSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tutor_schedule_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Tutor).WithMany(p => p.TutorSchedules)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_schedule_tutor");
        });

        modelBuilder.Entity<TutorVerificationRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tutor_verification_request_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.ReviewedByNavigation).WithMany(p => p.TutorVerificationRequests).HasConstraintName("fk_reviewed_by");

            entity.HasOne(d => d.Tutor).WithMany(p => p.TutorVerificationRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_verification_tutor");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.IsEmailVerified).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<UserOauthAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_oauth_account_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.User).WithMany(p => p.UserOauthAccounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_oauth_user");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_role_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_role");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user");
        });
    }

    /// <summary>
    /// Soft delete global query filter
    /// </summary>
    /// <param name="modelBuilder"></param>
    private static void SoftDeleteFilter(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType)) continue;
            var parameter = Expression.Parameter(entityType.ClrType, "p");
            var property = Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted));
            var condition = Expression.Equal(property, Expression.Constant(false));
            var lambda = Expression.Lambda(condition, parameter);

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }
    }
}