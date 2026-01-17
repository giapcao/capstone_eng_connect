using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("payment")]
public partial class Payment
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("order_id")]
    public Guid OrderId { get; set; }

    [Column("type")]
    [StringLength(30)]
    public string Type { get; set; } = null!;

    [Column("status")]
    [StringLength(30)]
    public string Status { get; set; } = null!;

    [Column("amount")]
    [Precision(12, 2)]
    public decimal Amount { get; set; }

    [Column("currency")]
    [StringLength(10)]
    public string Currency { get; set; } = null!;

    [Column("bank_transaction_id")]
    [StringLength(100)]
    public string? BankTransactionId { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("Payments")]
    public virtual Order Order { get; set; } = null!;
}
