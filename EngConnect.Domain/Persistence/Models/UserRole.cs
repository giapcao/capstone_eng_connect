using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class UserRole : AuditableEntity<Guid>
{
    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
