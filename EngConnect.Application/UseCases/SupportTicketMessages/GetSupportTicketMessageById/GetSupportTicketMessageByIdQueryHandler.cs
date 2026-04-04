using System.Net;
using EngConnect.Application.UseCases.SupportTicketMessages.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.SupportTicketMessages.GetSupportTicketMessageById;

public class GetSupportTicketMessageByIdQueryHandler : IQueryHandler<GetSupportTicketMessageByIdQuery, GetSupportTicketMessageResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetSupportTicketMessageByIdQueryHandler> _logger;

    public GetSupportTicketMessageByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetSupportTicketMessageByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<GetSupportTicketMessageResponse>> HandleAsync(
        GetSupportTicketMessageByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetSupportTicketMessageByIdQueryHandler: {@query}", query);
        try
        {
            var supportTicketMessage = await _unitOfWork.GetRepository<SupportTicketMessage, Guid>()
                .FindByIdAsync(query.Id, cancellationToken: cancellationToken);

            if (supportTicketMessage == null)
            {
                _logger.LogWarning("SupportTicketMessage not found: {id}", query.Id);
                return Result.Failure<GetSupportTicketMessageResponse>(
                    HttpStatusCode.BadRequest,
                    CommonErrors.NotFound<SupportTicketMessage>("Support Ticket Message"));
            }

            var supportTicketMessageResponse = supportTicketMessage.Adapt<GetSupportTicketMessageResponse>();
            
            _logger.LogInformation("End GetSupportTicketMessageByIdQueryHandler");
            return Result.Success(supportTicketMessageResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetSupportTicketMessageByIdQueryHandler: {@Message}", ex.Message);
            return Result.Failure<GetSupportTicketMessageResponse>(
                HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}