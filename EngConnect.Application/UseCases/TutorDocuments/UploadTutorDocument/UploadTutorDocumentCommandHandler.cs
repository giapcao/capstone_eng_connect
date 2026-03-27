using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.TutorDocuments.UploadTutorDocument;

public class UploadTutorDocumentCommandHandler : ICommandHandler<UploadTutorDocumentCommand>
{
    private readonly ILogger<UploadTutorDocumentCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAwsStorageService _awsStorageService;

    public UploadTutorDocumentCommandHandler(
        ILogger<UploadTutorDocumentCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IAwsStorageService awsStorageService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _awsStorageService = awsStorageService;
    }

    public async Task<Result> HandleAsync(
        UploadTutorDocumentCommand command,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UploadTutorDocumentCommandHandler {@Command}", command);

        try
        {
            var tutorRepo = _unitOfWork.GetRepository<Tutor, Guid>();
            var documentRepo = _unitOfWork.GetRepository<TutorDocument, Guid>();

            var tutorExists = await tutorRepo.AnyAsync(x => x.Id == command.TutorId, cancellationToken);
            if (!tutorExists)
            {
                return Result.Failure(HttpStatusCode.NotFound, TutorErrors.TutorNotFound());
            }

            var uploadResult = await _awsStorageService.UploadFileAsync(
                command.File,
                command.TutorId,
                "TutorDocument",
                cancellationToken);

            if (uploadResult == null)
            {
                return Result.Failure(HttpStatusCode.BadRequest, CommonErrors.ValidationFailed("File"));
            }

            var entity = new TutorDocument
            {
                TutorId = command.TutorId,
                Name = string.IsNullOrWhiteSpace(command.Name) ? uploadResult.OriginalFileName : command.Name,
                DocType = command.DocType,
                Url = uploadResult.RelativePath,
                IssuedBy = command.IssuedBy,
                IssuedAt = command.IssuedAt,
                ExpiredAt = command.ExpiredAt,
                Status = string.IsNullOrWhiteSpace(command.Status) ? nameof(CommonStatus.Active) : command.Status
            };

            documentRepo.Add(entity);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End UploadTutorDocumentCommandHandler");
            return Result.Success(entity.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UploadTutorDocumentCommandHandler {@Message}", ex.Message);
            return Result.Failure<Guid>(
                HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
