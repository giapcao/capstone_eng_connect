using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class UserOauthAccount : AuditableEntity<Guid>
{
    public Guid UserId { get; set; }

    public string Provider { get; set; } = null!;

    public string ProviderUserId { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
