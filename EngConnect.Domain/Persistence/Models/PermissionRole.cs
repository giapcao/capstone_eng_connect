using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("permission_role")]
[Index("PermissionId", "RoleId", Name = "uq_permission_role_permission_role", IsUnique = true)]
public partial class PermissionRole
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("permission_id")]
    public Guid PermissionId { get; set; }

    [Column("role_id")]
    public Guid RoleId { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("PermissionId")]
    [InverseProperty("PermissionRoles")]
    public virtual Permission Permission { get; set; } = null!;

    [ForeignKey("RoleId")]
    [InverseProperty("PermissionRoles")]
    public virtual Role Role { get; set; } = null!;
}
