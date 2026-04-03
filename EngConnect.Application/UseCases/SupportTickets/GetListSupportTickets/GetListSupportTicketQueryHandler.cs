using System.Linq.Expressions;
using EngConnect.Application.UseCases.SupportTickets.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.SupportTickets.GetListSupportTickets;

public class GetListSupportTicketQueryHandler : IQueryHandler<GetListSupportTicketQuery, PaginationResult<GetSupportTicketResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetListSupportTicketQueryHandler> _logger;

    public GetListSupportTicketQueryHandler(IUnitOfWork unitOfWork, ILogger<GetListSupportTicketQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<PaginationResult<GetSupportTicketResponse>>> HandleAsync(
        GetListSupportTicketQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListSupportTicketQueryHandler: {@query}", query);
        try
        {
            var supportTicketRepository = _unitOfWork.GetRepository<SupportTicket, Guid>();
            
            var supportTickets = supportTicketRepository.FindAll(includes:l=> l.SupportTicketMessages);
            
            Expression<Func<SupportTicket, bool>> predicate = x => true;
            
            if (ValidationUtil.IsNotNullOrEmpty(query.Status))
            {
                predicate = predicate.CombineAndAlsoExpressions(x => query.Status.ToLower().Contains(x.Status.ToLower()));
            }
            
            if (ValidationUtil.IsNotNullOrEmpty(query.Type))
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.Type != null && query.Type.ToLower().Contains(x.Type.ToLower()));
            }
            
            if (query.CreatedBy.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.CreatedBy == query.CreatedBy.Value);
            }
            
            supportTickets = supportTickets.Where(predicate);
            
            supportTickets = supportTickets.ApplySorting(query.GetSortParams());

            var result =
                await supportTickets.ProjectToPaginatedListAsync<SupportTicket, GetSupportTicketResponse>
                    (query.GetPaginationParams());
            
            _logger.LogInformation("End GetListSupportTicketQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListSupportTicketQueryHandler: {@Message}", ex.Message);
            return Result.Failure<PaginationResult<GetSupportTicketResponse>>(
                System.Net.HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}