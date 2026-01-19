using System;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class OrderDetail : AuditableEntity<Guid>
{
    public Guid OrderId { get; set; }

    public Guid CourseId { get; set; }

    public decimal PriceAtPurchase { get; set; }

    public string? MetaData { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
