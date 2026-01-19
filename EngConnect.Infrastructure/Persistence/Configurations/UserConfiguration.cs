using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EngConnect.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id).HasName("user_pkey");

        builder.ToTable("user");

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("uuid_generate_v4()");

        builder.Property(e => e.FirstName)
            .HasMaxLength(100)
            .HasColumnName("first_name");

        builder.Property(e => e.LastName)
            .HasMaxLength(100)
            .HasColumnName("last_name");

        builder.Property(e => e.UserName)
            .HasMaxLength(100)
            .HasColumnName("user_name");

        builder.Property(e => e.Email)
            .HasMaxLength(255)
            .HasColumnName("email");

        builder.Property(e => e.Phone)
            .HasMaxLength(20)
            .HasColumnName("phone");

        builder.Property(e => e.AddressNum)
            .HasMaxLength(50)
            .HasColumnName("address_num");

        builder.Property(e => e.ProvinceId)
            .HasMaxLength(20)
            .HasColumnName("province_id");

        builder.Property(e => e.ProvinceName)
            .HasMaxLength(255)
            .HasColumnName("province_name");

        builder.Property(e => e.WardId)
            .HasMaxLength(20)
            .HasColumnName("ward_id");

        builder.Property(e => e.WardName)
            .HasMaxLength(255)
            .HasColumnName("ward_name");

        builder.Property(e => e.PasswordHash)
            .HasColumnName("password_hash");

        builder.Property(e => e.Status)
            .HasMaxLength(20)
            .HasColumnName("status");

        builder.Property(e => e.RefreshToken)
            .HasColumnName("refresh_token");

        builder.Property(e => e.IsEmailVerified)
            .HasColumnName("is_email_verified")
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

        builder.HasIndex(e => e.Email)
            .HasDatabaseName("user_email_key")
            .IsUnique();

        builder.HasOne(d => d.Student)
            .WithOne(p => p.User)
            .HasForeignKey<Student>(d => d.UserId)
            .HasConstraintName("fk_student_user");

        builder.HasOne(d => d.Tutor)
            .WithOne(p => p.User)
            .HasForeignKey<Tutor>(d => d.UserId)
            .HasConstraintName("fk_tutor_user");
    }
}

