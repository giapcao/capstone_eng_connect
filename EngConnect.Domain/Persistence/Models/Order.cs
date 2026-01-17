using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("order")]
public partial class Order
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("student_id")]
    public Guid StudentId { get; set; }

    [Column("status")]
    [StringLength(30)]
    public string Status { get; set; } = null!;

    [Column("total_amount")]
    [Precision(12, 2)]
    public decimal TotalAmount { get; set; }

    [Column("commission")]
    [Precision(12, 2)]
    public decimal? Commission { get; set; }

    [Column("currency")]
    [StringLength(10)]
    public string Currency { get; set; } = null!;

    [Column("payment_reference")]
    [StringLength(100)]
    public string? PaymentReference { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("meta_data")]
    public string? MetaData { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [InverseProperty("Order")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [ForeignKey("StudentId")]
    [InverseProperty("Orders")]
    public virtual User Student { get; set; } = null!;
}
