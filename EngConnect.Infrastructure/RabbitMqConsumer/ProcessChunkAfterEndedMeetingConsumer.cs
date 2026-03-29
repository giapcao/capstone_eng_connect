using EngConnect.Application.UseCases.AiSummerize.GetAiSummary;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.EventBus.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace EngConnect.Infrastructure.RabbitMqConsumer;

public class ProcessChunkAfterEndedMeetingConsumer : IConsumer<ProcessMeetingRecordingAfterEndedEvent>
{
    private readonly ILogger<ProcessChunkAfterEndedMeetingConsumer> _logger;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IRedisService _redisService;

    public ProcessChunkAfterEndedMeetingConsumer(IRedisService redisService,ILogger<ProcessChunkAfterEndedMeetingConsumer> logger,ICommandDispatcher commandDispatcher)
    {
        _logger = logger;
        _redisService = redisService;
        _commandDispatcher = commandDispatcher;
    }
    
    public async Task Consume(ConsumeContext<ProcessMeetingRecordingAfterEndedEvent> context)
    {
        var eventData = context.Message;
        _logger.LogInformation("Start ProcessChunkAfterEndedMeetingConsumer {@EventData}", eventData);
        try
        {
            var currentChunksCount = await _redisService.SortedSetLengthAsync(eventData.LessonId.ToString());
            if (currentChunksCount < eventData.TotalChunks)
            {
                throw new InvalidOperationException(
                    $"Chunks not ready for Lesson {eventData.LessonId}. Expected: {eventData.TotalChunks}, Current: {currentChunksCount}");
            }
            var command = new GetAiSummaryCommand { LessonId =  eventData.LessonId };
            await _commandDispatcher.DispatchAsync(command);
            _logger.LogInformation("End ProcessChunkAfterEndedMeetingConsumer");
        }  catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error consuming ProcessChunkAfterEndedMeetingConsumer for lesson {LessonId}",
                eventData.LessonId);
            throw;
        }
    }
}