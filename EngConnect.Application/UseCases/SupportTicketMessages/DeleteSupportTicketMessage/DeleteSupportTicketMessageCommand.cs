using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.SupportTicketMessages.DeleteSupportTicketMessage;

public class DeleteSupportTicketMessageCommand : ICommand
{
    public Guid Id { get; set; }
}