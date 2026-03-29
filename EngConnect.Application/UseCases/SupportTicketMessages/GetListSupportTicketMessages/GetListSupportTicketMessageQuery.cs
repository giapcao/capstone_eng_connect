using EngConnect.Application.UseCases.SupportTicketMessages.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;

namespace EngConnect.Application.UseCases.SupportTicketMessages.GetListSupportTicketMessages;

public record GetListSupportTicketMessageQuery : BaseQuery<PaginationResult<GetSupportTicketMessageResponse>>
{
    public Guid? TicketId { get; set; }
    
    public Guid? SenderId { get; set; }
}