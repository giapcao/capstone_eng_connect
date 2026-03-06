using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseResources.DeleteCourseResource;

public class DeleteCourseResourceCommandHandler : ICommandHandler<DeleteCourseResourceCommand>
{
    private readonly ILogger<DeleteCourseResourceCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCourseResourceCommandHandler(ILogger<DeleteCourseResourceCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(DeleteCourseResourceCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start DeleteCourseResourceCommandHandler {@Command}", command);
        try
        {
            var courseResourceRepo = _unitOfWork.GetRepository<CourseResource, Guid>();

            var courseResource = await courseResourceRepo.FindSingleAsync(
                x => x.Id == command.Id,
                cancellationToken: cancellationToken);
            if (courseResource == null)
            {
                _logger.LogWarning("CourseResource not found with ID: {Id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, new Error("CourseResourceNotFound", "Tài nguyên không tồn tại"));
            }

            courseResourceRepo.Delete(courseResource);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End DeleteCourseResourceCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in DeleteCourseResourceCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
