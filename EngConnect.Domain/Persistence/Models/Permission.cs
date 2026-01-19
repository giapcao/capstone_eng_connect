using System;
using System.Collections.Generic;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class Permission : AuditableEntity<Guid>
{
    public string Code { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<PermissionRole> PermissionRoles { get; set; } = new List<PermissionRole>();
}
