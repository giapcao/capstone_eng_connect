using System.Net;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.Constants;
using EngConnect.BuildingBlock.Infrastructure.Persistence;
using EngConnect.Domain.Abstraction;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using EngConnect.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Infrastructure.Persistence.Repositories;

public class OutboxEventRepository: GenericRepository<OutboxEvent, Guid>, IOutboxEventRepository
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OutboxEventRepository> _logger;
    private static readonly TimeSpan ProcessingTimeout = TimeSpan.FromMinutes(5); // Processing timeout in 5 minutes

    public OutboxEventRepository(ApplicationDbContext context, IUnitOfWork unitOfWork,
        ILogger<OutboxEventRepository> logger) :
        base(context)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<List<OutboxEvent>>> GetPendingOrRetryableEventsAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var lockExpiredAt = DateTime.UtcNow - ProcessingTimeout;
            await _unitOfWork.BeginTransactionAsync();

            var events = await _context.OutboxEvents
                .Where(e =>
                    e.OutboxStatus == OutboxStatus.Pending ||
                    (e.OutboxStatus == OutboxStatus.Failed && e.NextRetryAt <= DateTime.UtcNow) ||
                    (e.OutboxStatus == OutboxStatus.Processing && e.LockAt <= lockExpiredAt))
                .OrderBy(e => e.CreatedAt)
                .Take(100)
                .ToListAsync(cancellationToken);

            if (events.Count == 0)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return events;
            }

            var workerId = Guid.NewGuid();

            foreach (var e in events)
            {
                e.MarkAsProcessing(workerId);
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitTransactionAsync();

            return events;
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Error while GetPendingOrRetryableEventsAsync");
            await _unitOfWork.RollbackTransactionAsync();
            return Result.Failure<List<OutboxEvent>>(HttpStatusCode.InternalServerError,
                OutboxEventErrors.FailedToGetPendingOrRetryableEvents);
        }
    }


    public void MarkEventAsPublishedAsync(OutboxEvent @event, CancellationToken cancellationToken = default)
    {
        @event.MarkAsPublished();
    }

    public void MarkEventAsPublishedAsync(List<OutboxEvent> events, CancellationToken cancellationToken = default)
    {
        events.ForEach(x => x.MarkAsPublished());
    }

    public void MarkEventAsFailedAsync(OutboxEvent @event, int retryCount, DateTime nextRetryAt, string error,
        CancellationToken cancellationToken = default)
    {
        @event.MarkAsFailed(retryCount, nextRetryAt, error);
    }

    public void MarkEventAsDeadAsync(OutboxEvent @event, CancellationToken cancellationToken = default)
    {
        @event.MarkAsDead();
    }
}