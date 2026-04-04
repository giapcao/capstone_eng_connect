using EngConnect.Application.UseCases.SupportTickets.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;

namespace EngConnect.Application.UseCases.SupportTickets.GetListSupportTickets;

public record GetListSupportTicketQuery : BaseQuery<PaginationResult<GetSupportTicketResponse>>
{
    public string? Status { get; set; }
    
    public string? Type { get; set; }
    
    public Guid? CreatedBy { get; set; }
}