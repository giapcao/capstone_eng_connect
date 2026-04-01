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

namespace EngConnect.Application.UseCases.CourseVerificationRequests.CreateCourseVerificationRequest;

public class CreateCourseVerificationRequestCommandHandler : ICommandHandler<CreateCourseVerificationRequestCommand>
{
    private readonly ILogger<CreateCourseVerificationRequestCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCourseVerificationRequestCommandHandler(ILogger<CreateCourseVerificationRequestCommandHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(CreateCourseVerificationRequestCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start CreateCourseVerificationRequestCommandHandler {@Command}", command);
        try
        {
            // Check tutor verified
            var tutor = await _unitOfWork.GetRepository<Tutor, Guid>().FindSingleAsync(
                x => x.Id == command.TutorId, tracking: false, cancellationToken);
            if (ValidationUtil.IsNullOrEmpty(tutor))
            {
                _logger.LogWarning("Tutor not found with ID: {TutorId}", command.TutorId);
                return Result.Failure(HttpStatusCode.BadRequest, TutorErrors.TutorNotFound());
            }

            if (tutor.VerifiedStatus != nameof(TutorVerifiedStatus.Verified))
            {
                _logger.LogWarning("Tutor with ID: {TutorId} is not verified", command.TutorId);
                return Result.Failure(HttpStatusCode.BadRequest, TutorErrors.TutorProfileNotVerified());
            }

            var courseVerificationRequestRepo = _unitOfWork.GetRepository<CourseVerificationRequest, Guid>();
            var courseRepo = _unitOfWork.GetRepository<Course, Guid>();

            // Check if course exists
            var courseExists =
                await courseRepo.FindFirstAsync(x => x.Id == command.CourseId, 
                    tracking: true, cancellationToken);
            if (ValidationUtil.IsNullOrEmpty(courseExists))
            {
                _logger.LogWarning("Course not found with ID: {CourseId}", command.CourseId);
                return Result.Failure(HttpStatusCode.BadRequest, CourseErrors.CourseNotFound());
            }

            // Check module of course exists
            var moduleExists = await _unitOfWork.GetRepository<CourseCourseModule, Guid>()
                .FindAll(x => x.CourseId == command.CourseId)
                .ToListAsync(cancellationToken: cancellationToken);

            if (ValidationUtil.IsNullOrEmpty(moduleExists))
            {
                _logger.LogWarning("Module not found with ID: {ModuleId}", command.CourseId);
                return Result.Failure(HttpStatusCode.BadRequest, CourseErrors.CourseModuleNotFound());
            }

            // CHeck session of course exists
            var sessionExists = await _unitOfWork.GetRepository<CourseModuleCourseSession, Guid>()
                .FindAll(x => moduleExists
                    .Select(m => m.CourseModuleId).Contains(x.CourseModuleId))
                .ToListAsync(cancellationToken: cancellationToken);

            if (ValidationUtil.IsNullOrEmpty(sessionExists))
            {
                _logger.LogWarning("A module have at least 1 session");
                return Result.Failure(HttpStatusCode.BadRequest, CourseModuleErrors.CourseSessionNotFound());
            }

            // Check if verification request already exists for the course
            var existingRequest = await courseVerificationRequestRepo.FindFirstAsync(x =>
                    x.CourseId == command.CourseId,
                cancellationToken: cancellationToken);
            if (existingRequest != null)
            {
                _logger.LogWarning("Course verification request already exists for course ID: {CourseId}",
                    command.CourseId);
                return Result.Failure(HttpStatusCode.BadRequest,
                    CourseVerificationErrors.VerificationRequestAlreadyExists());
            }

            var courseVerificationRequest = new CourseVerificationRequest
            {
                CourseId = command.CourseId,
                Status = nameof(CourseVerificationStatus.Pending),
                SubmittedAt = DateTime.UtcNow
            };


            courseVerificationRequestRepo.Add(courseVerificationRequest);

            // Change course status to pending
            courseExists.Status = nameof(CourseStatus.Pending);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End CreateCourseVerificationRequestCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateCourseVerificationRequestCommandHandler: {Message}",
                ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}