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

namespace EngConnect.Application.UseCases.Students.UpdateAvatarStudent;

public class UpdateAvatarStudentCommandHandler : ICommandHandler<UpdateAvatarStudentCommand>
{
    private readonly ILogger<UpdateAvatarStudentCommandHandler> _logger;
    private readonly IAwsStorageService _awsStorageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisService _cache;
    private readonly RedisCacheSettings _settings;

    public UpdateAvatarStudentCommandHandler(IAwsStorageService awsStorageService,IOptions<RedisCacheSettings> settings, IRedisService cache, IUnitOfWork unitOfWork, ILogger<UpdateAvatarStudentCommandHandler> logger)
    {
        _awsStorageService = awsStorageService;
        _cache = cache;
        _unitOfWork = unitOfWork;
        _settings = settings.Value;
        _logger = logger;
    }
    public async Task<Result> HandleAsync(UpdateAvatarStudentCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateAvatarStudentCommand {@command}", command);
        try
        {
            var studentExist = await _unitOfWork.GetRepository<Student, Guid>()
                .FindByIdAsync(command.Id, cancellationToken: cancellationToken);
            if (studentExist == null)
            {
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<Student>("Id"));
            }
            
            var updateFileResponse = await _awsStorageService.UpdateFileAsync(command.File,command.Id, 
                nameof(PrefixFile.StudentAvatar),cancellationToken);
            
            if (updateFileResponse == null)
            {
                return Result.Failure(HttpStatusCode.BadRequest, CommonErrors.ValidationFailed("File"));
            }

            studentExist.Avatar = updateFileResponse.RelativePath;
            await _unitOfWork.SaveChangesAsync();

            var cacheKey = RedisKeyGenerator.GenerateStudentAvatarKey(studentExist.Id);
            _ = _cache.SetCacheAsync(cacheKey, studentExist.Avatar,
                TimeSpan.FromMinutes(_settings.SettingCacheExpirationMinutes), false);
            _logger.LogInformation("End UpdateAvatarStudentCommand {@command}", command);
            
            return Result.Success(updateFileResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateAvatarStudentCommandHandler {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
