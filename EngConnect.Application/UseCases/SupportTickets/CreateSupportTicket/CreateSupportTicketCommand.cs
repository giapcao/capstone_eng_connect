using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.SupportTickets.CreateSupportTicket;

public class CreateSupportTicketCommand : ICommand
{
    public Guid CreatedBy { get; set; }
    
    public string Subject { get; set; } = null!;
    
    public string? Description { get; set; }
    
    public string? Type { get; set; }
    
}