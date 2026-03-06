using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
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

    public CreateCourseCommandHandler(ILogger<CreateCourseCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(CreateCourseCommand command, CancellationToken cancellationToken = default)
    {
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
            
            //Create courseId for create other table that have relationship with course table
            var courseId = Guid.NewGuid();
            
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
                EstimatedTime = TimeSpan.FromMinutes(command.EstimatedTime),
                EstimatedTimeLesson = TimeSpan.FromMinutes(command.EstimatedTimeLesson),
                Price = command.Price,
                Currency = command.Currency,
                NumberOfSessions = command.NumberOfSessions,
                NumsSessionInWeek = command.NumsSessionInWeek,
                ThumbnailUrl = command.ThumbnailUrl,
                DemoVideoUrl = command.DemoVideoUrl,
                Status = nameof(CourseStatus.Draft),
                IsCertificate = command.IsCertificate,
                NumberOfEnrollment = 0,
                RatingAverage = 5,
                RatingCount = 0
            };

            courseRepo.Add(course);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End CreateCourseCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateCourseCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
