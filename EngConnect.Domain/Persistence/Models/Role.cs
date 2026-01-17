using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("role")]
[Index("Code", Name = "role_code_key", IsUnique = true)]
public partial class Role
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("code")]
    [StringLength(100)]
    public string Code { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [InverseProperty("Role")]
    public virtual ICollection<PermissionRole> PermissionRoles { get; set; } = new List<PermissionRole>();

    [InverseProperty("Role")]
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
