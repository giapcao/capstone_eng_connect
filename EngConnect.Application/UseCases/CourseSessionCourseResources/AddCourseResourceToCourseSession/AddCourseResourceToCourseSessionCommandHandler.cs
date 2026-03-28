using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseSessionCourseResources.AddCourseResourceToCourseSession;

public class AddCourseResourceToCourseSessionCommandHandler : ICommandHandler<AddCourseResourceToCourseSessionCommand>
{
    private readonly ILogger<AddCourseResourceToCourseSessionCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public AddCourseResourceToCourseSessionCommandHandler(
        ILogger<AddCourseResourceToCourseSessionCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(AddCourseResourceToCourseSessionCommand command,
        CancellationToken cancellationToken = default)
    {
        // Track if we are using a transaction
        Guid? transactionId = null;
        _logger.LogInformation("Start AddCourseResourceToCourseSessionCommandHandler {@Command}", command);
        try
        {
            var courseSessionCourseResourceRepo = _unitOfWork.GetRepository<CourseSessionCourseResource, Guid>();
            var courseSessionRepo = _unitOfWork.GetRepository<CourseSession, Guid>();
            var courseResourceRepo = _unitOfWork.GetRepository<CourseResource, Guid>();

            // Check if course session exists
            var courseSessionExists =
                await courseSessionRepo.AnyAsync(x => x.Id == command.CourseSessionId, cancellationToken);
            if (!courseSessionExists)
            {
                _logger.LogWarning("CourseSession not found with ID: {CourseSessionId}", command.CourseSessionId);
                return Result.Failure(HttpStatusCode.NotFound,
                    new Error("CourseSessionNotFound", "Buổi học không tồn tại"));
            }

            // Check if any course resource not found
            var ids = command.CourseResources
                .Select(x => x.CourseResourceId)
                .ToList();

            var existingIds = await courseResourceRepo.FindAll(x => ids.Contains(x.Id))
                .Select(x => x.Id)
                .ToListAsync(cancellationToken: cancellationToken);

            var notFoundIds = ids.Except(existingIds).ToList();
            if (notFoundIds.Any())
            {
                _logger.LogWarning("CourseResources not found with IDs: {CourseResourceIds}",
                    string.Join(", ", notFoundIds));
                return Result.Failure(HttpStatusCode.BadRequest,
                    new Error("CourseResourcesNotFound",
                        $"Tài nguyên không tồn tại với IDs: {string.Join(", ", notFoundIds)}"));
            }


            // Check if relationship already exists
            var existingRelations = await _unitOfWork
                .GetRepository<CourseSessionCourseResource, Guid>()
                .FindAll(x => x.CourseSessionId == command.CourseSessionId)
                .Select(x => x.CourseResourceId)
                .ToListAsync(cancellationToken: cancellationToken);

            var duplicatedIds = command.CourseResources
                .Where(cr => existingRelations.Any(x => x == cr.CourseResourceId))
                .ToList();
            if (duplicatedIds.Any())
            {
                _logger.LogWarning(
                    "One or more CourseResources already exist in CourseSession with IDs: {CourseResourceIds}",
                    string.Join(", ", duplicatedIds.Select(x => x.CourseResourceId)));
                return Result.Failure(HttpStatusCode.BadRequest, CourseResourceErrors.RelationshipExist());
            }

            // Begin transaction
            var transaction = await _unitOfWork.BeginTransactionAsync();
            transactionId = transaction.TransactionId;
            foreach (var courseResource in command.CourseResources)
            {
                var courseSessionCourseResource = new CourseSessionCourseResource
                {
                    Id = Guid.NewGuid(),
                    CourseSessionId = command.CourseSessionId,
                    CourseResourceId = courseResource.CourseResourceId
                };

                courseSessionCourseResourceRepo.Add(courseSessionCourseResource);
            }
            
            

            await _unitOfWork.SaveChangesAsync();
            
            // Commit transaction
            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                _logger.LogDebug("Committing transaction with {TransactionId}", transactionId);
                await _unitOfWork.CommitTransactionAsync();
            }

            _logger.LogInformation("End AddCourseResourceToCourseSessionCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in AddCourseResourceToCourseSessionCommandHandler: {Message}",
                ex.Message);
            // Rollback transaction if exists
            if (ValidationUtil.IsNotNullOrEmpty(transactionId))
            {
                _logger.LogDebug("Rolling back transaction with {TransactionId} due to error", transactionId);
                await _unitOfWork.RollbackTransactionAsync();
            }
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}