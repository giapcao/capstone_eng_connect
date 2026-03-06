using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.Domain.Abstraction;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Infrastructure.Services;

public class MessageBusWithOutboxService:IMessageBusWithOutboxService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBusService _messageBusService;
    private readonly ILogger<MessageBusWithOutboxService> _logger;

    public MessageBusWithOutboxService(IUnitOfWork unitOfWork, IMessageBusService messageBusService, ILogger<MessageBusWithOutboxService> logger)
    {
        _unitOfWork = unitOfWork;
        _messageBusService = messageBusService;
        _logger = logger;
    }

    public async Task PublishWithOutboxFallbackAsync<T>(T message, string aggregateType, Guid aggregateId,
        CancellationToken cancellationToken = default) where T : class
    {
        _logger.LogInformation("Start PublishWithOutboxFallbackAsync for message {@Message} ", message);
        try
        {
            // Try to publish directly to the message bus
            await _messageBusService.PublishAsync(message, cancellationToken);
                _logger.LogInformation("Successfully published message to message bus.");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to publish message , saving to outbox for retry later.");
            try
            {
                //Fallback: Save to outbox for retry later
                var outboxRepo = _unitOfWork.GetRepository<OutboxEvent, Guid>();
                var outboxEvent = OutboxEvent.CreateOutboxEvent(aggregateType, aggregateId, outboxRepo);
                outboxRepo.Add(outboxEvent);
                await _unitOfWork.SaveChangesAsync();
            }catch (Exception outboxEx)
            {
                _logger.LogError(outboxEx, "Failed to save message to outbox after publish failure.");
                throw; // Rethrow to ensure the original failure is known
            }
        }
    }
}