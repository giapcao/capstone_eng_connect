using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.TutorDocuments.RemoveTutorDocument;

public class RemoveTutorDocumentCommandHandler : ICommandHandler<RemoveTutorDocumentCommand>
{
    private readonly ILogger<RemoveTutorDocumentCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAwsStorageService _awsStorageService;

    public RemoveTutorDocumentCommandHandler(
        ILogger<RemoveTutorDocumentCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IAwsStorageService awsStorageService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _awsStorageService = awsStorageService;
    }

    public async Task<Result> HandleAsync(
        RemoveTutorDocumentCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start RemoveTutorDocumentCommandHandler {@Command}", command);

        try
        {
            var documentRepo = _unitOfWork.GetRepository<TutorDocument, Guid>();

            var entity = await documentRepo.FindFirstAsync(
                x => x.Id == command.DocumentId && x.TutorId == command.TutorId,
                cancellationToken: cancellationToken);

            if (entity is null)
            {
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<TutorDocument>("Id"));
            }

            if (!string.IsNullOrWhiteSpace(entity.Url))
            {
                var isDeleted = await _awsStorageService.DeleteFileAsync(entity.Url, cancellationToken);
                if (!isDeleted)
                {
                    return Result.Failure(HttpStatusCode.BadRequest, CommonErrors.ValidationFailed("File"));
                }
            }

            documentRepo.Delete(entity);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End RemoveTutorDocumentCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in RemoveTutorDocumentCommandHandler {@Message}", ex.Message);
            return Result.Failure(
                HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
