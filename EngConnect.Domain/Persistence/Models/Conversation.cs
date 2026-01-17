using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("conversation")]
[Index("TutorId", "StudentId", Name = "uq_conversation_tutor_student", IsUnique = true)]
public partial class Conversation
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("tutor_id")]
    public Guid TutorId { get; set; }

    [Column("student_id")]
    public Guid StudentId { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [InverseProperty("Conversation")]
    public virtual ICollection<ConversationMessage> ConversationMessages { get; set; } = new List<ConversationMessage>();

    [ForeignKey("StudentId")]
    [InverseProperty("Conversations")]
    public virtual Student Student { get; set; } = null!;

    [ForeignKey("TutorId")]
    [InverseProperty("Conversations")]
    public virtual Tutor Tutor { get; set; } = null!;
}
