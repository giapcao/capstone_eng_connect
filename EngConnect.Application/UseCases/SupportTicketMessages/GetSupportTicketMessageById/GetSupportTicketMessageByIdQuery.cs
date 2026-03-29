using EngConnect.Application.UseCases.SupportTicketMessages.Common;
using EngConnect.BuildingBlock.Application.Base;

namespace EngConnect.Application.UseCases.SupportTicketMessages.GetSupportTicketMessageById;

public record GetSupportTicketMessageByIdQuery : IQuery<GetSupportTicketMessageResponse>
{
    public Guid Id { get; set; }
}