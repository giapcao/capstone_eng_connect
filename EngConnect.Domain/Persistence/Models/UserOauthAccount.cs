using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("user_oauth_account")]
[Index("Provider", "ProviderUserId", Name = "uq_provider_user", IsUnique = true)]
public partial class UserOauthAccount
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("provider")]
    [StringLength(50)]
    public string Provider { get; set; } = null!;

    [Column("provider_user_id")]
    [StringLength(255)]
    public string ProviderUserId { get; set; } = null!;

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserOauthAccounts")]
    public virtual User User { get; set; } = null!;
}
