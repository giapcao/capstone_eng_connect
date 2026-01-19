using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class CommunityPostConfiguration : IEntityTypeConfiguration<CommunityPost>
{
    public void Configure(EntityTypeBuilder<CommunityPost> builder)
    {
        builder.HasKey(e => e.Id).HasName("community_post_pkey");

        builder.ToTable("community_post");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(e => e.AuthorId)
            .HasColumnName("author_id");

        builder.Property(e => e.Title)
            .HasMaxLength(255)
            .HasColumnName("title");

        builder.Property(e => e.Type)
            .HasMaxLength(50)
            .HasColumnName("type");

        builder.Property(e => e.Content)
            .HasColumnName("content");

        builder.Property(e => e.IsPinned)
            .HasColumnName("is_pinned")
            .HasDefaultValue(false);

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

