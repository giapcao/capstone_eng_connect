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

public class UpdateAvatarCommandHandler : ICommandHandler<UpdateAvatarStudentCommand>
{
    private readonly ILogger<UpdateAvatarCommandHandler> _logger;
    private readonly IAwsStorageService _awsStorageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRedisService _cache;
    private readonly RedisCacheSettings _settings;

    public UpdateAvatarCommandHandler(IAwsStorageService awsStorageService,IOptions<RedisCacheSettings> settings, IRedisService cache, IUnitOfWork unitOfWork, ILogger<UpdateAvatarCommandHandler> logger)
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
            
            var updateFileRequest = await _awsStorageService.UpdateFileAsync(command.File,command.Id, nameof(PrefixFile.
                Avatar),cancellationToken);
            
            if (updateFileRequest == null)
            {
                return Result.Failure(HttpStatusCode.BadRequest, CommonErrors.ValidationFailed("File"));
            }

            studentExist.Avatar = updateFileRequest.RelativePath;
            await _unitOfWork.SaveChangesAsync();
            
            _= _cache.SetCacheAsync(studentExist.Id.ToString(),studentExist.Avatar,TimeSpan.FromDays(_settings.SettingCacheExpirationDays),false);
            _logger.LogInformation("End UpdateAvatarStudentCommand {@command}", command);
            
            return Result.Success(updateFileRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateAvatarStudentCommandHandler {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}