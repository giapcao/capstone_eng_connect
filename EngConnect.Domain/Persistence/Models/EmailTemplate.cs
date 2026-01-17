using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("email_template")]
public partial class EmailTemplate
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("subject_template")]
    public string SubjectTemplate { get; set; } = null!;

    [Column("body_html_template")]
    public string? BodyHtmlTemplate { get; set; }

    [Column("body_text_template")]
    public string? BodyTextTemplate { get; set; }

    [Column("updated_by")]
    public Guid? UpdatedBy { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("UpdatedBy")]
    [InverseProperty("EmailTemplates")]
    public virtual User? UpdatedByNavigation { get; set; }
}
