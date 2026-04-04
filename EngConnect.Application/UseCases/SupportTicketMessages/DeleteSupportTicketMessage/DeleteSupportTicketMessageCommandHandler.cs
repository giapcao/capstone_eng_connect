using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.SupportTicketMessages.DeleteSupportTicketMessage;

public class DeleteSupportTicketMessageCommandHandler : ICommandHandler<DeleteSupportTicketMessageCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteSupportTicketMessageCommandHandler> _logger;

    public DeleteSupportTicketMessageCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteSupportTicketMessageCommandHandler> logger)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(DeleteSupportTicketMessageCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start DeleteSupportTicketMessageCommandHandler: {@command}", command);
        try
        {
            var supportTicketMessage = await _unitOfWork.GetRepository<SupportTicketMessage, Guid>().FindByIdAsync(command.Id, cancellationToken: cancellationToken);

            if (supportTicketMessage == null)
            {
                _logger.LogWarning("SupportTicketMessage not found: {id}", command.Id);
                return Result.Failure(HttpStatusCode.BadRequest, CommonErrors.NotFound<SupportTicketMessage>("SupportTicketMessage"));
            }

            _unitOfWork.GetRepository<SupportTicketMessage, Guid>().Delete(supportTicketMessage);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("End DeleteSupportTicketMessageCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in DeleteSupportTicketMessageCommandHandler: {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}