using System.Linq.Expressions;
using EngConnect.Application.UseCases.SupportTicketMessages.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.SupportTicketMessages.GetListSupportTicketMessages;

public class GetListSupportTicketMessageQueryHandler : IQueryHandler<GetListSupportTicketMessageQuery, PaginationResult<GetSupportTicketMessageResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetListSupportTicketMessageQueryHandler> _logger;

    public GetListSupportTicketMessageQueryHandler(IUnitOfWork unitOfWork, ILogger<GetListSupportTicketMessageQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<PaginationResult<GetSupportTicketMessageResponse>>> HandleAsync(
        GetListSupportTicketMessageQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListSupportTicketMessageQueryHandler: {@query}", query);
        try
        {
            var supportTicketMessageRepository = _unitOfWork.GetRepository<SupportTicketMessage, Guid>();
            
            var supportTicketMessages = supportTicketMessageRepository.FindAll();

            Expression<Func<SupportTicketMessage, bool>> predicate = x => true;
            
            if (ValidationUtil.IsNotNullOrEmpty(query.TicketId))
            { 
                predicate = predicate.CombineAndAlsoExpressions(x=>x.TicketId == query.TicketId);
            }

            if (ValidationUtil.IsNotNullOrEmpty(query.SenderId))
            {
                predicate = predicate.CombineAndAlsoExpressions(x=>x.SenderId == query.SenderId);
            }
            
            supportTicketMessages = supportTicketMessages.Where(predicate);
            
            supportTicketMessages = supportTicketMessages.ApplySorting(query.GetSortParams());

            var result =
                await supportTicketMessages.ProjectToPaginatedListAsync<SupportTicketMessage, GetSupportTicketMessageResponse>
                    (query.GetPaginationParams());
            
            _logger.LogInformation("End GetListSupportTicketMessageQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListSupportTicketMessageQueryHandler: {@Message}", ex.Message);
            return Result.Failure<PaginationResult<GetSupportTicketMessageResponse>>(
                System.Net.HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}