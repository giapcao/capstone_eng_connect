using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseVerificationRequests.DeleteCourseVerificationRequest;

public class DeleteCourseVerificationRequestCommandHandler : ICommandHandler<DeleteCourseVerificationRequestCommand>
{
    private readonly ILogger<DeleteCourseVerificationRequestCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCourseVerificationRequestCommandHandler(ILogger<DeleteCourseVerificationRequestCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(DeleteCourseVerificationRequestCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start DeleteCourseVerificationRequestCommandHandler {@Command}", command);
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

            courseVerificationRequestRepo.Delete(courseVerificationRequest);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End DeleteCourseVerificationRequestCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in DeleteCourseVerificationRequestCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
