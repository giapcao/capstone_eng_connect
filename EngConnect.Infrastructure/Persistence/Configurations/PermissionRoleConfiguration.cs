using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class PermissionRoleConfiguration : IEntityTypeConfiguration<PermissionRole>
{
    public void Configure(EntityTypeBuilder<PermissionRole> builder)
    {
        builder.HasKey(e => e.Id).HasName("permission_role_pkey");

        builder.ToTable("permission_role");

        builder.Property(e => e.Id)
            .HasColumnName("id");

        builder.Property(e => e.PermissionId)
            .HasColumnName("permission_id");

        builder.Property(e => e.RoleId)
            .HasColumnName("role_id");

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

        builder.HasIndex(e => new { e.PermissionId, e.RoleId })
            .HasDatabaseName("uq_permission_role_permission_role")
            .IsUnique();

        builder.HasOne(d => d.Permission)
            .WithMany(p => p.PermissionRoles)
            .HasForeignKey(d => d.PermissionId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_permission");

        builder.HasOne(d => d.Role)
            .WithMany(p => p.PermissionRoles)
            .HasForeignKey(d => d.RoleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_role");
    }
}

