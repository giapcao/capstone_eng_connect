using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Settings;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EngConnect.Application.UseCases.Courses.UpdateDemoVideoCourse;

public class UpdateDemoVideoCourseCommandHandler : ICommandHandler<UpdateDemoVideoCourseCommand>
{
    private readonly ILogger<UpdateDemoVideoCourseCommandHandler> _logger;
    private readonly IAwsStorageService _awsStorageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisService _cache;
    private readonly RedisCacheSettings _settings;

    public UpdateDemoVideoCourseCommandHandler(IAwsStorageService awsStorageService,
        IOptions<RedisCacheSettings> settings, IRedisService cache,
        IUnitOfWork unitOfWork, ILogger<UpdateDemoVideoCourseCommandHandler> logger)
    {
        _awsStorageService = awsStorageService;
        _cache = cache;
        _unitOfWork = unitOfWork;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<Result> HandleAsync(UpdateDemoVideoCourseCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateDemoVideoCourseCommand {@command}", command);
        try
        {
            var courseExist = await _unitOfWork.GetRepository<Course, Guid>()
                .FindByIdAsync(command.Id, cancellationToken: cancellationToken);
            if (courseExist == null)
            {
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<Course>("Id"));
            }

            var updateFileResponse = await _awsStorageService.UpdateFileAsync(
                command.File, command.Id, nameof(PrefixFile.CourseDemoVideo), cancellationToken);

            if (updateFileResponse == null)
            {
                return Result.Failure(HttpStatusCode.BadRequest, CommonErrors.ValidationFailed("File"));
            }

            courseExist.DemoVideoUrl = updateFileResponse.RelativePath;
            
            // Set course status to Draft when update demo video, to make sure the course will be reviewed by admin before publish
            courseExist.Status = nameof(CourseStatus.Draft);
            await _unitOfWork.SaveChangesAsync();

            var cacheKey = RedisKeyGenerator.GenerateCourseDemoVideoKey(courseExist.Id);
            _ = _cache.SetCacheAsync(cacheKey, courseExist.DemoVideoUrl,
                TimeSpan.FromMinutes(_settings.SettingCacheExpirationMinutes), false);

            _logger.LogInformation("End UpdateDemoVideoCourseCommand {@command}", command);
            return Result.Success(updateFileResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateDemoVideoCourseCommandHandler {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
