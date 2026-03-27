using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseResources.CreateCourseResource;

public class CreateCourseResourceCommandHandler : ICommandHandler<CreateCourseResourceCommand>
{
    private readonly ILogger<CreateCourseResourceCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAwsStorageService _awsStorageService;

    public CreateCourseResourceCommandHandler(ILogger<CreateCourseResourceCommandHandler> logger, IUnitOfWork unitOfWork, IAwsStorageService awsStorageService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _awsStorageService = awsStorageService;
    }

    public async Task<Result> HandleAsync(CreateCourseResourceCommand command, CancellationToken cancellationToken = default)
    {
        //Track if we are using a transaction
        Guid? transactionId = null;
        _logger.LogInformation("Start CreateCourseResourceCommandHandler {@Command}", command);
        try
        {
            var courseResourceRepo = _unitOfWork.GetRepository<CourseResource, Guid>();
            var courseSessionRepo = _unitOfWork.GetRepository<CourseSession, Guid>();
            var courseSessionCourseResourceRepo = _unitOfWork.GetRepository<CourseSessionCourseResource, Guid>();

            // Check tutor 
            var tutorExists = await _unitOfWork.GetRepository<Tutor, Guid>().FindFirstAsync(
                x => x.Id == command.TutorId, 
                cancellationToken: cancellationToken);
            if (ValidationUtil.IsNullOrEmpty(tutorExists))
            {
                _logger.LogWarning("Tutor not found with ID: {TutorId}", command.TutorId);
                return Result.Failure(HttpStatusCode.BadRequest, TutorErrors.TutorNotFound());
            }
            // Check course session exists
            var courseSessionExists = await courseSessionRepo.AnyAsync(x => x.Id == command.CourseSessionId, cancellationToken);
            if (!courseSessionExists)
            {
                _logger.LogWarning("Course session not found with ID: {CourseSessionId}", command.CourseSessionId);
                return Result.Failure(HttpStatusCode.BadRequest, new Error("CourseSessionNotFound", "Session không tồn tại"));
            }
            
            // Begin transaction
            var transaction = await _unitOfWork.BeginTransactionAsync();
            transactionId = transaction.TransactionId;
            
            // Create course resource 
            var courseResourceUrl = await _awsStorageService.UploadFileAsync(command.ResourceFile, tutorExists.Id,
                nameof(PrefixFile.CourseResource), cancellationToken);

            var courseResource = new CourseResource
            {
                Id = Guid.NewGuid(),
                TutorId = tutorExists.Id,
                Title = command.Title,
                Url = courseResourceUrl.RelativePath,
                ResourceType = command.ResourceType,
                CreatedAt = DateTime.UtcNow
            };
            
            courseResourceRepo.Add(courseResource);
            
            // Add course resource to course session
            var courseSessionCourseResource = new CourseSessionCourseResource
            {
                Id = Guid.NewGuid(),
                CourseSessionId = command.CourseSessionId,
                CourseResourceId = courseResource.Id
            };
            courseSessionCourseResourceRepo.Add(courseSessionCourseResource);
            
            await _unitOfWork.SaveChangesAsync();

            // Commit transaction
            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                _logger.LogDebug("Committing transaction with {TransactionId}", transactionId);
                await _unitOfWork.CommitTransactionAsync();
            }
            _logger.LogInformation("End CreateCourseResourceCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateCourseResourceCommandHandler: {Message}", ex.Message);
            // Rollback transaction if exists
            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                _logger.LogDebug("Rolling back transaction with {TransactionId} due to error", transactionId);
                await _unitOfWork.RollbackTransactionAsync();
            }
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}