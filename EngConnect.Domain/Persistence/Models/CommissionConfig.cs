using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class CommissionConfig : AuditableEntity<Guid>
{
    public string Name { get; set; } = null!;

    public decimal CommissionPercent { get; set; }

    public DateTime ApplyFrom { get; set; }

    public DateTime? ApplyTo { get; set; }
}
