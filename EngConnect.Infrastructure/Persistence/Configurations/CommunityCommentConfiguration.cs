using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class CommunityCommentConfiguration : IEntityTypeConfiguration<CommunityComment>
{
    public void Configure(EntityTypeBuilder<CommunityComment> builder)
    {
        builder.HasKey(e => e.Id).HasName("community_comment_pkey");

        builder.ToTable("community_comment");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()");

        builder.Property(e => e.PostId)
            .HasColumnName("post_id");

        builder.Property(e => e.AuthorId)
            .HasColumnName("author_id");

        builder.Property(e => e.ParentCommentId)
            .HasColumnName("parent_comment_id");

        builder.Property(e => e.Content)
            .HasColumnName("content");

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

