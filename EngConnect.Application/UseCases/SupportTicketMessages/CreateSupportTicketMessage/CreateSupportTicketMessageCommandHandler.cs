using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.SupportTicketMessages.CreateSupportTicketMessage;

public class CreateSupportTicketMessageCommandHandler : ICommandHandler<CreateSupportTicketMessageCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateSupportTicketMessageCommandHandler> _logger;

    public CreateSupportTicketMessageCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateSupportTicketMessageCommandHandler> logger)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> HandleAsync(CreateSupportTicketMessageCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start CreateSupportTicketMessageCommandHandler: {@command}", command);
        try
        {
            var supportTicketMessage = command.Adapt<SupportTicketMessage>();
            
            var ticketExists = await _unitOfWork.GetRepository<SupportTicket, Guid>()
                .AnyAsync(x => x.Id == supportTicketMessage.TicketId, cancellationToken: cancellationToken);
            
            if (!ticketExists)
            {
                _logger.LogWarning("SupportTicket does not exist: {ticketId}", command.TicketId);
                return Result.Failure(HttpStatusCode.BadRequest, CommonErrors.NotFound<SupportTicket>("Support Ticket"));
            }
            
            var senderExists = await _unitOfWork.GetRepository<User, Guid>()
                .AnyAsync(x => x.Id == supportTicketMessage.SenderId, cancellationToken: cancellationToken);
            
            if (!senderExists)
            {
                _logger.LogWarning("Sender user does not exist: {senderId}", command.SenderId);
                return Result.Failure(HttpStatusCode.BadRequest, CommonErrors.NotFound<User>("Sender"));
            }
            
            _unitOfWork.GetRepository<SupportTicketMessage, Guid>().Add(supportTicketMessage);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("End CreateSupportTicketMessageCommandHandler");
            
            return Result.Success(supportTicketMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateSupportTicketMessageCommandHandler {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}