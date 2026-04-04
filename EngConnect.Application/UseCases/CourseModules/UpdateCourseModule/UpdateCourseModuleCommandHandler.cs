using System.Net;
using EngConnect.Application.UseCases.CourseModules.Common;
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

namespace EngConnect.Application.UseCases.CourseModules.UpdateCourseModule;

public class UpdateCourseModuleCommandHandler : ICommandHandler<UpdateCourseModuleCommand, GetCourseModuleResponse>
{
    private readonly ILogger<UpdateCourseModuleCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseModuleCommandHandler(ILogger<UpdateCourseModuleCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetCourseModuleResponse>> HandleAsync(UpdateCourseModuleCommand command,
        CancellationToken cancellationToken = default)
    {
        Guid? transactionId = null;
        _logger.LogInformation("Start UpdateCourseModuleCommandHandler {@Command}", command);
        try
        {
            var courseModuleRepo = _unitOfWork.GetRepository<CourseModule, Guid>();
            var courseCourseModuleRepo = _unitOfWork.GetRepository<CourseCourseModule, Guid>();

            var courseModule = await courseModuleRepo.FindSingleAsync(
                x => x.Id == command.Id,
                cancellationToken: cancellationToken);
            if (courseModule == null)
            {
                _logger.LogWarning("CourseModule not found with ID: {Id}", command.Id);
                return Result.Failure<GetCourseModuleResponse>(HttpStatusCode.NotFound,
                    new Error("CourseModuleNotFound", "Course module not found"));
            }

            var relations = await courseCourseModuleRepo.FindAll(x => x.CourseModuleId == command.Id)
                .Include(x => x.Course)
                .ToListAsync(cancellationToken);

            if (relations.Any(x => x.Course.Status == nameof(CourseStatus.Published)))
            {
                _logger.LogWarning("CourseModule with ID: {Id} cannot be updated because it's in use by a Published course", command.Id);
                return Result.Failure<GetCourseModuleResponse>(HttpStatusCode.BadRequest,
                    CourseModuleErrors.CourseModuleIsInUse());
            }

            if (command.CourseId.HasValue && relations.All(x => x.CourseId != command.CourseId.Value))
            {
                return Result.Failure<GetCourseModuleResponse>(HttpStatusCode.BadRequest,
                    CommonErrors.ValidationFailed("CourseId does not reference this module"));
            }

            var transaction = await _unitOfWork.BeginTransactionAsync();
            transactionId = transaction.TransactionId;

            var newModule = new CourseModule
            {
                Id = Guid.NewGuid(),
                TutorId = courseModule.TutorId,
                ParentModuleId = courseModule.Id,
                Title = command.Title,
                Description = command.Description,
                Outcomes = command.Outcomes
            };

            courseModuleRepo.Add(newModule);

            var targetRelations = command.CourseId.HasValue
                ? relations.Where(x => x.CourseId == command.CourseId.Value).ToList()
                : relations;

            foreach (var relation in targetRelations)
            {
                relation.CourseModuleId = newModule.Id;
                courseCourseModuleRepo.Update(relation);
            }

            await _unitOfWork.SaveChangesAsync();

            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                await _unitOfWork.CommitTransactionAsync();
            }

            var targetCourseId = targetRelations.Select(x => x.CourseId).FirstOrDefault();
            return Result.Success(new GetCourseModuleResponse
            {
                Id = newModule.Id,
                CourseId = targetCourseId,
                ParentModuleId = newModule.ParentModuleId,
                Title = newModule.Title,
                Description = newModule.Description,
                Outcomes = newModule.Outcomes,
                CreatedAt = newModule.CreatedAt,
                UpdatedAt = newModule.UpdatedAt
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateCourseModuleCommandHandler: {Message}", ex.Message);
            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                await _unitOfWork.RollbackTransactionAsync();
            }

            return Result.Failure<GetCourseModuleResponse>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
