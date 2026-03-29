using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.SupportTickets.CreateSupportTicket;

public class CreateSupportTicketCommandHandler : ICommandHandler<CreateSupportTicketCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateSupportTicketCommandHandler> _logger;

    public CreateSupportTicketCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateSupportTicketCommandHandler> logger)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> HandleAsync(CreateSupportTicketCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start CreateSupportTicketCommandHandler: {@command}", command);
        try
        {
            var supportTicket = command.Adapt<SupportTicket>();
            
            var userExists = await _unitOfWork.GetRepository<User, Guid>()
                .AnyAsync(x => x.Id == supportTicket.CreatedBy, cancellationToken: cancellationToken);
            
            if (!userExists)
            {
                _logger.LogWarning("CreatedBy user does not exist: {createdBy}", command.CreatedBy);
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<User>("User"));
            }
            
            supportTicket.Status = nameof(SupportTicketStatus.Open);
            _unitOfWork.GetRepository<SupportTicket, Guid>().Add(supportTicket);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("End CreateSupportTicketCommandHandler");
            
            return Result.Success(supportTicket);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateSupportTicketCommandHandler {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}