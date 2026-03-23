using System.Net;
using EngConnect.Application.UseCases.SupportTickets.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.SupportTickets.GetSupportTicketById;

public class GetSupportTicketByIdQueryHandler : IQueryHandler<GetSupportTicketByIdQuery, GetSupportTicketResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetSupportTicketByIdQueryHandler> _logger;

    public GetSupportTicketByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetSupportTicketByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<GetSupportTicketResponse>> HandleAsync(
        GetSupportTicketByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetSupportTicketByIdQueryHandler: {@query}", query);
        try
        {
            var supportTicket = await _unitOfWork.GetRepository<SupportTicket, Guid>()
                .FindByIdAsync(query.Id, cancellationToken: cancellationToken);

            if (supportTicket == null)
            {
                _logger.LogWarning("SupportTicket not found: {id}", query.Id);
                return Result.Failure<GetSupportTicketResponse>(
                    HttpStatusCode.NotFound,
                    CommonErrors.NotFound<SupportTicket>("Support Ticket"));
            }

            var supportTicketResponse = supportTicket.Adapt<GetSupportTicketResponse>();
            
            _logger.LogInformation("End GetSupportTicketByIdQueryHandler");
            return Result.Success(supportTicketResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetSupportTicketByIdQueryHandler: {@Message}", ex.Message);
            return Result.Failure<GetSupportTicketResponse>(
                HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}