using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class UserOauthAccountConfiguration : IEntityTypeConfiguration<UserOauthAccount>
{
    public void Configure(EntityTypeBuilder<UserOauthAccount> builder)
    {
        builder.HasKey(e => e.Id).HasName("user_oauth_account_pkey");

        builder.ToTable("user_oauth_account");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("uuid_generate_v4()");

        builder.Property(e => e.UserId)
            .HasColumnName("user_id");

        builder.Property(e => e.Provider)
            .HasMaxLength(50)
            .HasColumnName("provider");

        builder.Property(e => e.ProviderUserId)
            .HasMaxLength(255)
            .HasColumnName("provider_user_id");

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

        builder.HasIndex(e => new { e.Provider, e.ProviderUserId })
            .HasDatabaseName("uq_provider_user")
            .IsUnique();

        builder.HasOne(d => d.User)
            .WithMany(p => p.UserOauthAccounts)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("fk_oauth_user");
    }
}

