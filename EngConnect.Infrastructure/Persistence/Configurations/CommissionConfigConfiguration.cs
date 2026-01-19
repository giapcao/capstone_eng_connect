using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class CommissionConfigConfiguration : IEntityTypeConfiguration<CommissionConfig>
{
    public void Configure(EntityTypeBuilder<CommissionConfig> builder)
    {
        builder.HasKey(e => e.Id).HasName("commission_config_pkey");

        builder.ToTable("commission_config");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(e => e.Name)
            .HasMaxLength(100)
            .HasColumnName("name");

        builder.Property(e => e.CommissionPercent)
            .HasPrecision(5, 2)
            .HasColumnName("commission_percent");

        builder.Property(e => e.ApplyFrom)
            .HasColumnName("apply_from");

        builder.Property(e => e.ApplyTo)
            .HasColumnName("apply_to");

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
    }
}

