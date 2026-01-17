using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("course")]
public partial class Course
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("tutor_id")]
    public Guid TutorId { get; set; }

    [Column("parent_course_id")]
    public Guid? ParentCourseId { get; set; }

    [Column("title")]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    [Column("short_description")]
    public string? ShortDescription { get; set; }

    [Column("full_description")]
    public string? FullDescription { get; set; }

    [Column("outcomes")]
    public string? Outcomes { get; set; }

    [Column("level")]
    [StringLength(50)]
    public string? Level { get; set; }

    [Column("estimated_time")]
    public TimeSpan? EstimatedTime { get; set; }

    [Column("estimated_time_lesson")]
    public TimeSpan? EstimatedTimeLesson { get; set; }

    [Column("price")]
    [Precision(12, 2)]
    public decimal? Price { get; set; }

    [Column("currency")]
    [StringLength(10)]
    public string? Currency { get; set; }

    [Column("number_of_sessions")]
    public int? NumberOfSessions { get; set; }

    [Column("nums_session_in_week")]
    public int? NumsSessionInWeek { get; set; }

    [Column("thumbnail_url")]
    public string? ThumbnailUrl { get; set; }

    [Column("demo_video_url")]
    public string? DemoVideoUrl { get; set; }

    [Column("number_of_enrollment")]
    public int? NumberOfEnrollment { get; set; }

    [Column("rating_average")]
    [Precision(3, 2)]
    public decimal? RatingAverage { get; set; }

    [Column("rating_count")]
    public int? RatingCount { get; set; }

    [Column("status")]
    [StringLength(30)]
    public string? Status { get; set; }

    [Column("is_certificate")]
    public bool? IsCertificate { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [InverseProperty("Course")]
    public virtual ICollection<ActualSchedule> ActualSchedules { get; set; } = new List<ActualSchedule>();

    [InverseProperty("Course")]
    public virtual ICollection<CourseCategory> CourseCategories { get; set; } = new List<CourseCategory>();

    [InverseProperty("Course")]
    public virtual ICollection<CourseEnrollment> CourseEnrollments { get; set; } = new List<CourseEnrollment>();

    [InverseProperty("Course")]
    public virtual ICollection<CourseModule> CourseModules { get; set; } = new List<CourseModule>();

    [InverseProperty("Course")]
    public virtual ICollection<CourseReview> CourseReviews { get; set; } = new List<CourseReview>();

    [InverseProperty("Course")]
    public virtual ICollection<CourseVerificationRequest> CourseVerificationRequests { get; set; } = new List<CourseVerificationRequest>();

    [InverseProperty("ParentCourse")]
    public virtual ICollection<Course> InverseParentCourse { get; set; } = new List<Course>();

    [InverseProperty("Course")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [ForeignKey("ParentCourseId")]
    [InverseProperty("InverseParentCourse")]
    public virtual Course? ParentCourse { get; set; }

    [InverseProperty("Course")]
    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();

    [ForeignKey("TutorId")]
    [InverseProperty("Courses")]
    public virtual Tutor Tutor { get; set; } = null!;
}
