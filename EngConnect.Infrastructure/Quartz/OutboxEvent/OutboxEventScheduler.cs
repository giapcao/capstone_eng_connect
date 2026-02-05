using System.Net;
using EngConnect.Application.Abstraction;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Models.Quartz;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.Domain.Persistence.Models;
using EngConnect.Domain.Settings;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EngConnect.Infrastructure.Quartz.OutboxEvent;

public class OutboxEventScheduler : IOutboxEventScheduler
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISchedulerService _schedulerService;
    private readonly ILogger<OutboxEventScheduler> _logger;
    private readonly IMapper _mapper;
    private readonly AppSettings _appSettings;

    public OutboxEventScheduler(IUnitOfWork unitOfWork, ISchedulerService schedulerService,
        ILogger<OutboxEventScheduler> logger, IMapper mapper, IOptions<AppSettings> appSettings)
    {
        _unitOfWork = unitOfWork;
        _schedulerService = schedulerService;
        _logger = logger;
        _mapper = mapper;
        _appSettings = appSettings.Value;
    }

    public async Task<Result> ScheduleOutboxEventAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var jobName = "OutboxEventJob";
            var scheduleJobModel = new QuartzRecurringIntervalJobModel
            {
                JobName = jobName,
                IntervalInSeconds = _appSettings.OutboxEventIntervalTimeInSeconds,
                StartAt = DateTimeOffset.UtcNow.AddSeconds(10) // Start 10 seconds after the application starts.
            };

            var scheduleResult = await _schedulerService
                .CreateRecurringIntervalJobAsync<OutboxEventJob>(scheduleJobModel);

            if (!scheduleResult.IsSuccess)
            {
                _logger.LogWarning("Failed to schedule OutboxEventJob: {Error}", scheduleResult.Error);
                return Result.Failure(HttpStatusCode.InternalServerError, scheduleResult.Error!);
            }

            var jobValue = scheduleResult.Value!;

            var existingJob = await _unitOfWork.GetRepository<ScheduleJobTracking, Guid>()
                .FindSingleAsync(j => j.JobName == jobName, cancellationToken: cancellationToken);

            if (existingJob != null)
            {
                existingJob.MapFrom(jobValue);
                _unitOfWork.GetRepository<ScheduleJobTracking, Guid>().Update(existingJob);
                await _unitOfWork.SaveChangesAsync();
                return Result.Success();
            }

            var job = _mapper.Map<ScheduleJobTracking>(jobValue);
            _unitOfWork.GetRepository<ScheduleJobTracking, Guid>().Add(job);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Scheduled OutboxEventJob successfully");
            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Error while scheduling OutboxEventJob");
            return Result.Failure(HttpStatusCode.InternalServerError,
                new Error("OutboxEventScheduler.ScheduleFailed", "Không thể lên lịch công việc OutboxEventJob"));
        }
    }
}