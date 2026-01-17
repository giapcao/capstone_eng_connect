using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("tutor_schedule")]
public partial class TutorSchedule
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("tutor_id")]
    public Guid TutorId { get; set; }

    [Column("weekday")]
    [StringLength(20)]
    public string? Weekday { get; set; }

    [Column("start_time")]
    public TimeOnly StartTime { get; set; }

    [Column("end_time")]
    public TimeOnly EndTime { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("TutorId")]
    [InverseProperty("TutorSchedules")]
    public virtual Tutor Tutor { get; set; } = null!;
}
