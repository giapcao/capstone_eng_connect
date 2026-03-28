using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseVerificationRequests.UpdateCourseVerificationRequest;

public class UpdateCourseVerificationRequestCommandHandler : ICommandHandler<UpdateCourseVerificationRequestCommand>
{
    private readonly ILogger<UpdateCourseVerificationRequestCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseVerificationRequestCommandHandler(ILogger<UpdateCourseVerificationRequestCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(UpdateCourseVerificationRequestCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateCourseVerificationRequestCommandHandler {@Command}", command);
        try
        {
            var courseVerificationRequestRepo = _unitOfWork.GetRepository<CourseVerificationRequest, Guid>();

            var courseVerificationRequest = await courseVerificationRequestRepo.FindSingleAsync(
                x => x.Id == command.Id,
                cancellationToken: cancellationToken);
            if (courseVerificationRequest == null)
            {
                _logger.LogWarning("CourseVerificationRequest not found with ID: {Id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseVerificationRequestNotFound", "Yêu cầu xác thực không tồn tại"));
            }

            courseVerificationRequest.Status = command.Status;
            courseVerificationRequest.ReviewedAt = DateTime.Now;
            courseVerificationRequest.ReviewedBy = command.UserId;

            if (command.Status == nameof(CourseVerificationStatus.Rejected))
            {
                courseVerificationRequest.RejectionReason = command.RejectionReason;
            }

            courseVerificationRequestRepo.Update(courseVerificationRequest);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End UpdateCourseVerificationRequestCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateCourseVerificationRequestCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
