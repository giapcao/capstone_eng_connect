using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicket;

public class UpdateSupportTicketCommand : ICommand
{
    public Guid Id { get; set; }
    
    public string Subject { get; set; } = null!;
    
    public string? Description { get; set; }
    
    public string? Type { get; set; }
}