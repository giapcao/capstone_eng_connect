using System.Net;
using EngConnect.Application.UseCases.CourseResources.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseResources.UpdateCourseResource;

public class UpdateCourseResourceCommandHandler : ICommandHandler<UpdateCourseResourceCommand, GetCourseResourceResponse>
{
    private readonly ILogger<UpdateCourseResourceCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseResourceCommandHandler(ILogger<UpdateCourseResourceCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetCourseResourceResponse>> HandleAsync(UpdateCourseResourceCommand command,
        CancellationToken cancellationToken = default)
    {
        Guid? transactionId = null;
        _logger.LogInformation("Start UpdateCourseResourceCommandHandler {@Command}", command);
        try
        {
            var courseResourceRepo = _unitOfWork.GetRepository<CourseResource, Guid>();
            var courseSessionCourseResourceRepo = _unitOfWork.GetRepository<CourseSessionCourseResource, Guid>();

            var courseResource = await courseResourceRepo.FindAll(x => x.Id == command.Id)
                .Include(x => x.CourseSessionCourseResources)
                .FirstOrDefaultAsync(cancellationToken);
            if (courseResource == null)
            {
                _logger.LogWarning("CourseResource not found with ID: {Id}", command.Id);
                return Result.Failure<GetCourseResourceResponse>(HttpStatusCode.NotFound,
                    new Error("CourseResourceNotFound", "Tai nguyen khong ton tai"));
            }

            var transaction = await _unitOfWork.BeginTransactionAsync();
            transactionId = transaction.TransactionId;

            var newResource = new CourseResource
            {
                Id = Guid.NewGuid(),
                TutorId = courseResource.TutorId,
                ParentResourceId = courseResource.Id,
                Title = command.Title,
                ResourceType = command.ResourceType,
                Url = command.Url ?? courseResource.Url,
                Status = command.Status
            };

            courseResourceRepo.Add(newResource);

            foreach (var relation in courseResource.CourseSessionCourseResources)
            {
                relation.CourseResourceId = newResource.Id;
                courseSessionCourseResourceRepo.Update(relation);
            }

            await _unitOfWork.SaveChangesAsync();

            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                await _unitOfWork.CommitTransactionAsync();
            }

            _logger.LogInformation("End UpdateCourseResourceCommandHandler");
            return Result.Success(new GetCourseResourceResponse
            {
                Id = newResource.Id,
                TutorId = newResource.TutorId ?? Guid.Empty,
                ParentResourceId = newResource.ParentResourceId,
                Title = newResource.Title,
                ResourceType = newResource.ResourceType,
                Url = newResource.Url,
                Status = newResource.Status,
                CreatedAt = newResource.CreatedAt,
                UpdatedAt = newResource.UpdatedAt
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateCourseResourceCommandHandler: {Message}", ex.Message);
            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                await _unitOfWork.RollbackTransactionAsync();
            }

            return Result.Failure<GetCourseResourceResponse>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
