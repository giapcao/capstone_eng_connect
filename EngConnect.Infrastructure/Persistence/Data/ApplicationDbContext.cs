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

    public virtual DbSet<Conversation> Conversations { get; set; }

    public virtual DbSet<ConversationMessage> ConversationMessages { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<CourseCategory> CourseCategories { get; set; }

    public virtual DbSet<CourseEnrollment> CourseEnrollments { get; set; }

    public virtual DbSet<EnrollmentSlot> EnrollmentSlots { get; set; }

    public virtual DbSet<CourseModule> CourseModules { get; set; }

    public virtual DbSet<CourseResource> CourseResources { get; set; }

    public virtual DbSet<CourseReview> CourseReviews { get; set; }

    public virtual DbSet<CourseSession> CourseSessions { get; set; }

    public virtual DbSet<CourseVerificationRequest> CourseVerificationRequests { get; set; }

    public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<LessonHomework> LessonHomeworks { get; set; }

    public virtual DbSet<LessonRecord> LessonRecords { get; set; }

    public virtual DbSet<LessonRescheduleRequest> LessonRescheduleRequests { get; set; }

    public virtual DbSet<LessonScript> LessonScripts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OutboxEvent> OutboxEvents { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<PermissionRole> PermissionRoles { get; set; }

    // 260327: Remove Quiz and related table

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
    
    //260201: Add ScheduleJobTracking DbSet
    public virtual DbSet<ScheduleJobTracking> ScheduleJobTrackings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SoftDeleteFilter(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder
            .HasPostgresExtension("pgcrypto")
            .HasPostgresExtension("uuid-ossp");
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