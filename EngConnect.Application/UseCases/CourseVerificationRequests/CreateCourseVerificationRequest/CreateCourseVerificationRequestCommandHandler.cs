using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseVerificationRequests.CreateCourseVerificationRequest;

public class CreateCourseVerificationRequestCommandHandler : ICommandHandler<CreateCourseVerificationRequestCommand>
{
    private readonly ILogger<CreateCourseVerificationRequestCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCourseVerificationRequestCommandHandler(ILogger<CreateCourseVerificationRequestCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(CreateCourseVerificationRequestCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start CreateCourseVerificationRequestCommandHandler {@Command}", command);
        try
        {
            var courseVerificationRequestRepo = _unitOfWork.GetRepository<CourseVerificationRequest, Guid>();
            var courseRepo = _unitOfWork.GetRepository<Course, Guid>();

            // Check if course exists
            var courseExists = await courseRepo.AnyAsync(x => x.Id == command.CourseId, cancellationToken);
            if (!courseExists)
            {
                _logger.LogWarning("Course not found with ID: {CourseId}", command.CourseId);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseNotFound", "Khóa học không tồn tại"));
            }

            var courseVerificationRequest = new CourseVerificationRequest
            {
                CourseId = command.CourseId,
                Status = command.Status,
                SubmittedAt = command.SubmittedAt ?? DateTime.UtcNow
            };

            courseVerificationRequestRepo.Add(courseVerificationRequest);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End CreateCourseVerificationRequestCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateCourseVerificationRequestCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
