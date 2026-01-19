using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.Id).HasName("order_pkey");

        builder.ToTable("order");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(e => e.StudentId)
            .HasColumnName("student_id");

        builder.Property(e => e.Status)
            .HasMaxLength(30)
            .HasColumnName("status");

        builder.Property(e => e.TotalAmount)
            .HasPrecision(12, 2)
            .HasColumnName("total_amount");

        builder.Property(e => e.Commission)
            .HasPrecision(12, 2)
            .HasColumnName("commission");

        builder.Property(e => e.Currency)
            .HasMaxLength(10)
            .HasColumnName("currency");

        builder.Property(e => e.PaymentReference)
            .HasMaxLength(100)
            .HasColumnName("payment_reference");

        builder.Property(e => e.Description)
            .HasColumnName("description");

        builder.Property(e => e.MetaData)
            .HasColumnName("meta_data");

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

        builder.HasOne(d => d.Student)
            .WithMany(p => p.Orders)
            .HasForeignKey(d => d.StudentId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_order_student");
    }
}

