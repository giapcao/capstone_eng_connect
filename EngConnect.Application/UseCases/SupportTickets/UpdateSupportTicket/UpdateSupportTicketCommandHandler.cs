using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.SupportTickets.UpdateSupportTicket;

public class UpdateSupportTicketCommandHandler : ICommandHandler<UpdateSupportTicketCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateSupportTicketCommandHandler> _logger;

    public UpdateSupportTicketCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateSupportTicketCommandHandler> logger)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(UpdateSupportTicketCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateSupportTicketCommandHandler: {@command}", command);
        try
        {
            var supportTicket = await _unitOfWork.GetRepository<SupportTicket, Guid>().FindByIdAsync(command.Id, cancellationToken: cancellationToken);

            if (supportTicket == null)
            {
                _logger.LogWarning("SupportTicket not found: {id}", command.Id);
                return Result.Failure(HttpStatusCode.BadRequest, CommonErrors.NotFound<SupportTicket>("SupportTicket"));
            }
            
            command.Adapt(supportTicket);
            
            _unitOfWork.GetRepository<SupportTicket, Guid>().Update(supportTicket);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("End UpdateSupportTicketCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateSupportTicketCommandHandler: {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}