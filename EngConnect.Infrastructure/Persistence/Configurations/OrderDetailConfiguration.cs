using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.HasKey(e => e.Id).HasName("order_detail_pkey");

        builder.ToTable("order_detail");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(e => e.OrderId)
            .HasColumnName("order_id");

        builder.Property(e => e.CourseId)
            .HasColumnName("course_id");

        builder.Property(e => e.PriceAtPurchase)
            .HasPrecision(12, 2)
            .HasColumnName("price_at_purchase");

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

        builder.HasOne(d => d.Course)
            .WithMany(p => p.OrderDetails)
            .HasForeignKey(d => d.CourseId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_order_detail_course");

        builder.HasOne(d => d.Order)
            .WithMany(p => p.OrderDetails)
            .HasForeignKey(d => d.OrderId)
            .HasConstraintName("fk_order_detail_order");
    }
}

