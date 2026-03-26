using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.SupportTicketMessages.UpdateSupportTicketMessage;

public class UpdateSupportTicketMessageCommandHandler : ICommandHandler<UpdateSupportTicketMessageCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateSupportTicketMessageCommandHandler> _logger;

    public UpdateSupportTicketMessageCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateSupportTicketMessageCommandHandler> logger)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(UpdateSupportTicketMessageCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateSupportTicketMessageCommandHandler: {@command}", command);
        try
        {
            var supportTicketMessage = await _unitOfWork.GetRepository<SupportTicketMessage, Guid>().FindByIdAsync(command.Id, cancellationToken: cancellationToken);

            if (supportTicketMessage == null)
            {
                _logger.LogWarning("SupportTicketMessage not found: {id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<SupportTicketMessage>("SupportTicketMessage"));
            }
            
            command.Adapt(supportTicketMessage);
            
            _unitOfWork.GetRepository<SupportTicketMessage, Guid>().Update(supportTicketMessage);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("End UpdateSupportTicketMessageCommandHandler");
            return Result.Success(supportTicketMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateSupportTicketMessageCommandHandler: {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}