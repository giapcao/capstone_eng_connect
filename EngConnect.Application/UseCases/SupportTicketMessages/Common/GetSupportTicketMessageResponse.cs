namespace EngConnect.Application.UseCases.SupportTicketMessages.Common;

public class GetSupportTicketMessageResponse
{
    public Guid Id { get; set; }
    
    public Guid TicketId { get; set; }
    
    public Guid SenderId { get; set; }
    
    public string Message { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
}