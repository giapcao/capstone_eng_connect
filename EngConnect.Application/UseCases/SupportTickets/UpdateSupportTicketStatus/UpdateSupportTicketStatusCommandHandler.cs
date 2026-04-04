using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicketStatus;

public class UpdateSupportTicketStatusCommandHandler : ICommandHandler<UpdateSupportTicketStatusCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateSupportTicketStatusCommandHandler> _logger;

    public UpdateSupportTicketStatusCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateSupportTicketStatusCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> HandleAsync(UpdateSupportTicketStatusCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateSupportTicketStatusCommandHandler: {@command}", command);
        try
        {
            var supportTicket = await _unitOfWork.GetRepository<SupportTicket, Guid>()
                .FindByIdAsync(command.Id, cancellationToken: cancellationToken);

            if (supportTicket == null)
            {
                _logger.LogWarning("SupportTicket not found: {id}", command.Id);
                return Result.Failure(HttpStatusCode.BadRequest, CommonErrors.NotFound<SupportTicket>("SupportTicket"));
            }

            supportTicket.Status = command.Status.Trim();
            supportTicket.ClosedAt = string.Equals(supportTicket.Status, "Closed", StringComparison.OrdinalIgnoreCase)
                ? DateTime.UtcNow
                : null;

            _unitOfWork.GetRepository<SupportTicket, Guid>().Update(supportTicket);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End UpdateSupportTicketStatusCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateSupportTicketStatusCommandHandler: {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}