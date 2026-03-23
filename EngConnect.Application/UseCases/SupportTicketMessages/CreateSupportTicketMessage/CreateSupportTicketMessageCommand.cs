using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.SupportTicketMessages.CreateSupportTicketMessage;

public class CreateSupportTicketMessageCommand : ICommand
{
    public Guid TicketId { get; set; }
    
    public Guid SenderId { get; set; }
    
    public string Message { get; set; } = null!;
}