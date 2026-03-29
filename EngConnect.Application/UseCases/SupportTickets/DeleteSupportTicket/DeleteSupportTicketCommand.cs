using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.SupportTickets.DeleteSupportTicket;

public class DeleteSupportTicketCommand : ICommand
{
    public Guid Id { get; set; }
}