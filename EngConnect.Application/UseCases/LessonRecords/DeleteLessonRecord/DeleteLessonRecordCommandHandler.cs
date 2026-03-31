using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.LessonRecords.DeleteLessonRecord;

public class DeleteLessonRecordCommandHandler : ICommandHandler<DeleteLessonRecordCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteLessonRecordCommandHandler> _logger;

    public DeleteLessonRecordCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteLessonRecordCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> HandleAsync(DeleteLessonRecordCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start DeleteLessonRecordCommandHandler: {@command}", command);
        try
        {
            var lessonRecord = await _unitOfWork.GetRepository<LessonRecord, Guid>()
                .FindByIdAsync(command.Id, true, cancellationToken, x => x.LessonScript!);

            if (lessonRecord == null)
            {
                _logger.LogWarning("LessonRecord not found: {id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound,
                    CommonErrors.NotFound<LessonRecord>("Bản ghi bài học"));
            }

            if (lessonRecord.LessonScript != null)
            {
                _unitOfWork.GetRepository<LessonScript, Guid>().Delete(lessonRecord.LessonScript);
            }

            _unitOfWork.GetRepository<LessonRecord, Guid>().Delete(lessonRecord);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End DeleteLessonRecordCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in DeleteLessonRecordCommandHandler: {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
