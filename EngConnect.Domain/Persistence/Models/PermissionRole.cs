using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class PermissionRole : AuditableEntity<Guid>
{
    public Guid PermissionId { get; set; }

    public Guid RoleId { get; set; }

    public virtual Permission Permission { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
