using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class Payment : AuditableEntity<Guid>
{
    public Guid OrderId { get; set; }

    public string Type { get; set; } = null!;

    public string Status { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Currency { get; set; } = null!;

    public string? BankTransactionId { get; set; }

    public virtual Order Order { get; set; } = null!;
}
