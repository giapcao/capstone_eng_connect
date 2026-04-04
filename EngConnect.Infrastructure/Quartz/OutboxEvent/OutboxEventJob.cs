using System.Text.Json;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.Domain.Abstraction;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace EngConnect.Infrastructure.Quartz.OutboxEvent;

public class OutboxEventJob : IJob
{
    private readonly IOutboxEventRepository _outboxEventRepository;
    private readonly ILogger<OutboxEventJob> _logger;
    private readonly IMessageBusService _messageBusService;
    private const int MaxRetryCount = 5;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ScheduleJobSettings _scheduleJobSettings;

    public OutboxEventJob(IOutboxEventRepository outboxEventRepository, ILogger<OutboxEventJob> logger,
        IMessageBusService messageBusService, IUnitOfWork unitOfWork, IOptions<ScheduleJobSettings> scheduleJobSettings)
    {
        _outboxEventRepository = outboxEventRepository;
        _logger = logger;
        _messageBusService = messageBusService;
        _unitOfWork = unitOfWork;
        _scheduleJobSettings = scheduleJobSettings.Value;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        // _logger.LogInformation("Start OutboxEventJob");
        if (!_scheduleJobSettings.EnableAllJobs || !_scheduleJobSettings.EnableOutboxEventJob)
        {
            _logger.LogInformation("OutboxEventJob is unable via configuration.");
            return;
        }

        // Get a job
        var jobName = context.JobDetail.Key.Name;
        var scheduleJobTracking = await _unitOfWork.GetRepository<ScheduleJobTracking, Guid>()
            .FindSingleAsync(x => x.JobName == jobName);

        if (scheduleJobTracking == null)
        {
            _logger.LogInformation("Schedule job tracking not found for job name: {JobName}", jobName);
            return;
        }

        scheduleJobTracking.IncrementRunCount();

        try
        {
            // Get pending or retryable events
            var events = await _outboxEventRepository.GetPendingOrRetryableEventsAsync();

            var eventList = events.Value;
            if (events.IsFailure || eventList == null)
            {
                _logger.LogInformation("Failed to get pending or retryable events: {Error}", events.Error);
                scheduleJobTracking.UpdateFireTimes(context.FireTimeUtc, context.NextFireTimeUtc, false);
                _unitOfWork.GetRepository<ScheduleJobTracking, Guid>().Update(scheduleJobTracking);
                await _unitOfWork.SaveChangesAsync();
                return;
            }

            if (eventList.Count == 0)
            {
                _logger.LogInformation("No pending or retryable events found");
                scheduleJobTracking.UpdateFireTimes(context.FireTimeUtc, context.NextFireTimeUtc, true);
                _unitOfWork.GetRepository<ScheduleJobTracking, Guid>().Update(scheduleJobTracking);
                await _unitOfWork.SaveChangesAsync();
                return;
            }

            _logger.LogInformation("Found {EventCount} pending or retryable events", eventList.Count);

            // Publish events
            foreach (var @event in eventList)
            {
                try
                {
                    // 1. Get event type
                    var eventType = MasstransitHelper.GetValidEventType(@event.EventType);

                    if (eventType == null)
                    {
                        _logger.LogInformation("Invalid event type: {EventType}", @event.EventType);
                        throw new ArgumentException("Invalid event type");
                    }

                    // 2. Deserialize event data
                    var eventData = JsonSerializer.Deserialize(@event.EventData.ToString()!, eventType);
                    if (eventData == null)
                    {
                        _logger.LogInformation("Failed to deserialize event data: {EventData}", @event.EventData);
                        throw new ArgumentException("Failed to deserialize event data");
                    }


                    // 3. Publish event
                    _logger.LogWarning("alolololo");
                    await _messageBusService.PublishAsync(eventData, eventType);
                    _logger.LogInformation("Successfully published event: {Event}", @event);
                    _outboxEventRepository.MarkEventAsPublishedAsync(@event);
                }
                catch (Exception e)
                {
                    _logger.LogInformation(e, "Failed to publish event: {Error}", e.Message);

                    // 4. Handle publish failure
                    HandlePublishFailure(@event, e.Message);
                }
            }

            // Save changes
            scheduleJobTracking.UpdateFireTimes(context.FireTimeUtc, context.NextFireTimeUtc, true);
            _unitOfWork.GetRepository<ScheduleJobTracking, Guid>().Update(scheduleJobTracking);

            _unitOfWork.GetRepository<Domain.Persistence.Models.OutboxEvent, Guid>().UpdateRange(eventList);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("End OutboxEventJob");
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Error executing OutboxEventJob: {Error}", e.Message);
            scheduleJobTracking.UpdateFireTimes(context.FireTimeUtc, context.NextFireTimeUtc, false);
            _unitOfWork.GetRepository<ScheduleJobTracking, Guid>().Update(scheduleJobTracking);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    private void HandlePublishFailure(Domain.Persistence.Models.OutboxEvent @event, string error)
    {
        if (@event == null) throw new ArgumentNullException(nameof(@event));
        if (@event.RetryCount >= MaxRetryCount)
        {
            _outboxEventRepository.MarkEventAsDeadAsync(@event);
        }
        else
        {
            var nextRetryAt = DateTime.UtcNow.AddSeconds(BackOffSeconds(@event.RetryCount));
            _outboxEventRepository.MarkEventAsFailedAsync(@event, @event.RetryCount + 1, nextRetryAt, error);
        }
    }

    private static long BackOffSeconds(int retryCount)
    {
        // Exponential backoff: 5s, 25s, 2m, 10m, etc.
        return (long)Math.Min(600, Math.Pow(5, retryCount));
    }
}