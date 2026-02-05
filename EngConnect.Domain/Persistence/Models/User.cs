using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EngConnect.BuildingBlock.Contracts.Abstraction.Entities;

namespace EngConnect.Domain.Persistence.Models;

public class User : AuditableEntity<Guid>
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? AddressNum { get; set; }

    public string? ProvinceId { get; set; }

    public string? ProvinceName { get; set; }

    public string? WardId { get; set; }

    public string? WardName { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? RefreshToken { get; set; }

    public bool? IsEmailVerified { get; set; }

    [InverseProperty("ReviewedByNavigation")]
    public virtual ICollection<CourseVerificationRequest> CourseVerificationRequests { get; set; } = new List<CourseVerificationRequest>();

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
    
    public static User Create(string firstName, string lastName, string userName, string email,
    string? phone, string? addressNum, string? provinceId, string? provinceName,
        string? wardId, string? wardName, string password, string status)
    {
        return new User
        {
            FirstName = firstName,
            LastName = lastName,
            UserName = userName,
            Email = email,
            Phone = phone,
            AddressNum = addressNum,
            ProvinceId = provinceId,
            ProvinceName = provinceName,
            WardId = wardId,
            WardName = wardName,
            PasswordHash = password,
            Status = status,
            IsEmailVerified = false
        };
    }
}
