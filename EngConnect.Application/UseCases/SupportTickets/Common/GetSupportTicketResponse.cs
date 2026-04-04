using EngConnect.Application.UseCases.SupportTicketMessages.Common;
using EngConnect.Domain.Persistence.Models;

namespace EngConnect.Application.UseCases.SupportTickets.Common;

public class GetSupportTicketResponse
{
    public Guid Id { get; set; }
    
    public Guid CreatedBy { get; set; }
    
    public string Subject { get; set; } = null!;
    
    public string? Description { get; set; }
    
    public string? Type { get; set; }
    
    public string Status { get; set; } = null!;
    
    public DateTime? ClosedAt { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<GetSupportTicketMessageResponse>? SupportTicketMessages { get; set; } 
}