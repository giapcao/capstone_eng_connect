using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseResources.UpdateCourseResource;

public class UpdateCourseResourceCommandHandler : ICommandHandler<UpdateCourseResourceCommand>
{
    private readonly ILogger<UpdateCourseResourceCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseResourceCommandHandler(ILogger<UpdateCourseResourceCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(UpdateCourseResourceCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateCourseResourceCommandHandler {@Command}", command);
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

            courseResource.Title = command.Title;
            courseResource.ResourceType = command.ResourceType;
            courseResource.Url = command.Url;
            courseResource.Status = command.Status;

            courseResourceRepo.Update(courseResource);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End UpdateCourseResourceCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateCourseResourceCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
