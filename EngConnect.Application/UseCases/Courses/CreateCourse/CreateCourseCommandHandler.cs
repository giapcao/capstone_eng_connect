using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Courses.CreateCourse;

public class CreateCourseCommandHandler : ICommandHandler<CreateCourseCommand>
{
    private readonly ILogger<CreateCourseCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAwsStorageService _awsStorageService;

    public CreateCourseCommandHandler(ILogger<CreateCourseCommandHandler> logger, IUnitOfWork unitOfWork, IAwsStorageService awsStorageService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _awsStorageService = awsStorageService;
    }

    public async Task<Result> HandleAsync(CreateCourseCommand command, CancellationToken cancellationToken = default)
    {
        //Track if we are using a transaction
        Guid? transactionId = null;
        _logger.LogInformation("Start CreateCourseCommandHandler {@Command}", command);
        try
        {
            var courseRepo = _unitOfWork.GetRepository<Course, Guid>();
            var tutorRepo = _unitOfWork.GetRepository<Tutor, Guid>();

            // Check if tutor exists
            var tutorExists = await tutorRepo.AnyAsync(x => x.Id == command.TutorId, cancellationToken);
            if (!tutorExists)
            {
                _logger.LogWarning("Tutor not found with ID: {TutorId}", command.TutorId);
                return Result.Failure(HttpStatusCode.NotFound, new Error("TutorNotFound", "Giáo viên không tồn tại"));
            }

            // Check if parent course exists (if provided)
            if (command.ParentCourseId.HasValue)
            {
                var parentCourseExists = await courseRepo.AnyAsync(x => x.Id == command.ParentCourseId.Value, cancellationToken);
                if (!parentCourseExists)
                {
                    _logger.LogWarning("Parent course not found with ID: {ParentCourseId}", command.ParentCourseId);
                    return Result.Failure(HttpStatusCode.NotFound, new Error("ParentCourseNotFound", "Khóa học cha không tồn tại"));
                }
            }
            
            var transaction = await _unitOfWork.BeginTransactionAsync();
            transactionId = transaction.TransactionId;
            //Create courseId for create other table that have relationship with course table
            var courseId = Guid.NewGuid();
            
            // Upload thumbnail if provided
            string? thumbnailUrl = null;
            if (command.ThumbnailFile != null)
            {
                var thumbnailUploadResult = await _awsStorageService.UploadFileAsync(
                    command.ThumbnailFile, 
                    command.TutorId, 
                    "course/thumbnail", 
                    cancellationToken);
                
                if (thumbnailUploadResult == null)
                {
                    _logger.LogWarning("Failed to upload thumbnail file");
                    await _unitOfWork.RollbackTransactionAsync();
                    return Result.Failure(HttpStatusCode.BadRequest, new Error("ThumbnailUploadFailed", "Không thể upload ảnh thumbnail"));
                }
                
                thumbnailUrl = thumbnailUploadResult.RelativePath;
            }
            
            // Upload demo video if provided
            string? demoVideoUrl = null;
            if (command.DemoVideoFile != null)
            {
                var videoUploadResult = await _awsStorageService.UploadFileAsync(
                    command.DemoVideoFile, 
                    command.TutorId, 
                    "course/demo-video", 
                    cancellationToken);
                
                if (videoUploadResult == null)
                {
                    _logger.LogWarning("Failed to upload demo video file");
                    await _unitOfWork.RollbackTransactionAsync();
                    return Result.Failure(HttpStatusCode.BadRequest, new Error("DemoVideoUploadFailed", "Không thể upload video demo"));
                }
                
                demoVideoUrl = videoUploadResult.RelativePath;
            }
            
            //Check if category exists 
            if (command.CategoryIds is { Length: > 0 })
            {
                //Check if categories exist
                var categories = await _unitOfWork.GetRepository<Category, Guid>()
                    .FindAll(x => command.CategoryIds.Contains(x.Id))
                    .ToListAsync(cancellationToken);

                if (categories.Count != command.CategoryIds.Length)
                {
                    _logger.LogWarning("One or more categories not found with IDs: {CategoryIds}", command.CategoryIds);
                    return Result.Failure(HttpStatusCode.NotFound, CourseErrors.CategoryNotFound());
                }
                
                //Create categories
                var courseCategories = categories.Select(category => new CourseCategory
                {
                    CourseId = courseId,
                    CategoryId = category.Id
                }).ToList();
                
                _unitOfWork.GetRepository<CourseCategory, Guid>().AddRange(courseCategories);
            }

            var course = new Course
            {
                Id = courseId,
                TutorId = command.TutorId,
                ParentCourseId = command.ParentCourseId,
                Title = command.Title,
                ShortDescription = command.ShortDescription,
                FullDescription = command.FullDescription,
                Outcomes = command.Outcomes,
                Level = command.Level,
                EstimatedTime = TimeSpan.Zero,
                EstimatedTimeLesson = TimeSpan.FromMinutes(command.EstimatedTimeLesson),
                Price = command.Price,
                Currency = command.Currency ?? "vnd",
                NumberOfSessions = 0,
                NumsSessionInWeek = command.NumsSessionInWeek,
                ThumbnailUrl = thumbnailUrl,
                DemoVideoUrl = demoVideoUrl,
                Status = nameof(CourseStatus.Draft),
                IsCertificate = command.IsCertificate,
                NumberOfEnrollment = 0,
                RatingAverage = 5,
                RatingCount = 0
            };

            courseRepo.Add(course);
            await _unitOfWork.SaveChangesAsync();
            
            //Commit transaction if we started one
            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                _logger.LogDebug("Committing transaction with {TransactionId}", transactionId);
                await _unitOfWork.CommitTransactionAsync();
            }

            _logger.LogInformation("End CreateCourseCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateCourseCommandHandler: {Message}", ex.Message);
            //Rollback transaction if we started one
            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                _logger.LogDebug("Rolling back transaction with {TransactionId}", transactionId);
                await _unitOfWork.RollbackTransactionAsync();
            }
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
