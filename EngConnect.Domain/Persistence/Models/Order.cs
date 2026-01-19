using System;
using System.Collections.Generic;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class Order : AuditableEntity<Guid>
{
    public Guid StudentId { get; set; }

    public string Status { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public decimal? Commission { get; set; }

    public string Currency { get; set; } = null!;

    public string? PaymentReference { get; set; }

    public string? Description { get; set; }

    public string? MetaData { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual User Student { get; set; } = null!;
}
