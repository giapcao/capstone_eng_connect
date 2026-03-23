using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicketStatus;

public class UpdateSupportTicketStatusCommand : ICommand
{
    public Guid Id { get; set; }
    
    public string Status { get; set; } = null!;
}