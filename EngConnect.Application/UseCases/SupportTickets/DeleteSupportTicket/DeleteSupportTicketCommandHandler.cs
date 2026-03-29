using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.SupportTickets.DeleteSupportTicket;

public class DeleteSupportTicketCommandHandler : ICommandHandler<DeleteSupportTicketCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteSupportTicketCommandHandler> _logger;

    public DeleteSupportTicketCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteSupportTicketCommandHandler> logger)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(DeleteSupportTicketCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start DeleteSupportTicketCommandHandler: {@command}", command);
        try
        {
            var supportTicket = await _unitOfWork.GetRepository<SupportTicket, Guid>().FindByIdAsync(command.Id, cancellationToken: cancellationToken);

            if (supportTicket == null)
            {
                _logger.LogWarning("SupportTicket not found: {id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<SupportTicket>("SupportTicket"));
            }

            _unitOfWork.GetRepository<SupportTicket, Guid>().Delete(supportTicket);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("End DeleteSupportTicketCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in DeleteSupportTicketCommandHandler: {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}