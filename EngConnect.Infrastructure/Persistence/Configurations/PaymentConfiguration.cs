using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(e => e.Id).HasName("payment_pkey");

        builder.ToTable("payment");

        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.OrderId)
            .HasColumnName("order_id");

        builder.Property(e => e.Type)
            .HasMaxLength(30)
            .HasColumnName("type");

        builder.Property(e => e.Status)
            .HasMaxLength(30)
            .HasColumnName("status")
            .HasDefaultValueSql("'pending'::character varying");

        builder.Property(e => e.Amount)
            .HasPrecision(12, 2)
            .HasColumnName("amount");

        builder.Property(e => e.Currency)
            .HasMaxLength(10)
            .HasColumnName("currency");

        builder.Property(e => e.BankTransactionId)
            .HasMaxLength(100)
            .HasColumnName("bank_transaction_id");

        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("now()");

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("now()");

        builder.Property(e => e.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false);

        builder.Property(e => e.DeletedAt)
            .HasColumnName("deleted_at");

        builder.HasOne(d => d.Order)
            .WithMany(p => p.Payments)
            .HasForeignKey(d => d.OrderId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_payment_order");
    }
}

