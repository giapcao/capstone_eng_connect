using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngConnect.Domain.Persistence.Models
{
    [Table("actual_schedule")]
    public partial class ActualSchedule
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("tutor_id")]
        public Guid TutorId { get; set; }

        [Column("student_id")]
        public Guid? StudentId { get; set; }

        [Column("course_id")]
        public Guid? CourseId { get; set; }

        [Column("start_time")]
        public DateTime StartTime { get; set; }

        [Column("end_time")]
        public DateTime EndTime { get; set; }

        [Column("status")]
        [StringLength(20)]
        public string? Status { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("is_deleted")]
        public bool? IsDeleted { get; set; }

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        [ForeignKey("CourseId")]
        [InverseProperty("ActualSchedules")]
        public virtual Course? Course { get; set; }

        [ForeignKey("StudentId")]
        [InverseProperty("ActualSchedules")]
        public virtual Student? Student { get; set; }

        [ForeignKey("TutorId")]
        [InverseProperty("ActualSchedules")]
        public virtual Tutor Tutor { get; set; } = null!;
    }
}