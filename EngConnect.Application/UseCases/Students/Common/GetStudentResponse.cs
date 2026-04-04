using EngConnect.Application.UseCases.Users.Common;

namespace EngConnect.Application.UseCases.Students.Common;

public class GetStudentResponse
{
    public Guid Id {get; set;}
    
    public Guid UserId { get; set; }
    
    public string? Notes { get; set; }

    public string? School { get; set; }

    public string? Grade { get; set; }

    public string? Class { get; set; }
    
    public string? Avatar { get; set; }
    public List<string>? Tags { get; set; }

    public string? Status { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public UserInfo? User { get; set; }
}

