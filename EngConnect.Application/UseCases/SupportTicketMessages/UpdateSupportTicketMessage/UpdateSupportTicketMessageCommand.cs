using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.SupportTicketMessages.UpdateSupportTicketMessage;

public class UpdateSupportTicketMessageCommand : ICommand
{
    public Guid Id { get; set; }
    
    public string Message { get; set; } = null!;
}