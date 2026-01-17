using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EngConnect.Domain.Persistence.Models;

[Table("user")]
[Index("Email", Name = "user_email_key", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("first_name")]
    [StringLength(100)]
    public string? FirstName { get; set; }

    [Column("last_name")]
    [StringLength(100)]
    public string? LastName { get; set; }

    [Column("user_name")]
    [StringLength(100)]
    public string? UserName { get; set; }

    [Column("email")]
    [StringLength(255)]
    public string? Email { get; set; }

    [Column("phone")]
    [StringLength(20)]
    public string? Phone { get; set; }

    [Column("address_num")]
    [StringLength(50)]
    public string? AddressNum { get; set; }

    [Column("province_id")]
    [StringLength(20)]
    public string? ProvinceId { get; set; }

    [Column("province_name")]
    [StringLength(255)]
    public string? ProvinceName { get; set; }

    [Column("ward_id")]
    [StringLength(20)]
    public string? WardId { get; set; }

    [Column("ward_name")]
    [StringLength(255)]
    public string? WardName { get; set; }

    [Column("password_hash")]
    public string? PasswordHash { get; set; }

    [Column("status")]
    [StringLength(20)]
    public string? Status { get; set; }

    [Column("refresh_token")]
    public string? RefreshToken { get; set; }

    [Column("is_email_verified")]
    public bool? IsEmailVerified { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    [InverseProperty("ReviewedByNavigation")]
    public virtual ICollection<CourseVerificationRequest> CourseVerificationRequests { get; set; } = new List<CourseVerificationRequest>();

    [InverseProperty("UpdatedByNavigation")]
    public virtual ICollection<EmailTemplate> EmailTemplates { get; set; } = new List<EmailTemplate>();

    [InverseProperty("Student")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("User")]
    public virtual Student? Student { get; set; }

    [InverseProperty("Sender")]
    public virtual ICollection<SupportTicketMessage> SupportTicketMessages { get; set; } = new List<SupportTicketMessage>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SupportTicket> SupportTickets { get; set; } = new List<SupportTicket>();

    [InverseProperty("User")]
    public virtual Tutor? Tutor { get; set; }

    [InverseProperty("ReviewedByNavigation")]
    public virtual ICollection<TutorVerificationRequest> TutorVerificationRequests { get; set; } = new List<TutorVerificationRequest>();

    [InverseProperty("User")]
    public virtual ICollection<UserOauthAccount> UserOauthAccounts { get; set; } = new List<UserOauthAccount>();

    [InverseProperty("User")]
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
