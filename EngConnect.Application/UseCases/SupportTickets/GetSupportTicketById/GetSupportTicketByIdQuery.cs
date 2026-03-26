using EngConnect.Application.UseCases.SupportTickets.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.SupportTickets.GetSupportTicketById;

public record GetSupportTicketByIdQuery : IQuery<GetSupportTicketResponse>
{
    public Guid Id { get; set; }
}