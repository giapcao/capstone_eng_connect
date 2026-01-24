using System.Net;
using EngConnect.Application.UseCases.Students.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Students.DeleteStudent;

public class DeleteStudentCommandHandler : ICommandHandler<DeleteStudentCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteStudentCommandHandler> _logger;

    public DeleteStudentCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteStudentCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<Result> HandleAsync(DeleteStudentCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start DeleteStudentByIdHandler : {@command}",command);
        try
        {
            var studentExist = await _unitOfWork.GetRepository<Student, Guid>()
                .FindByIdAsync(command.Id, cancellationToken: cancellationToken);
            
            if (studentExist == null || studentExist.UserId != command.UserId)
            {
                _logger.LogWarning( "student không tồn tại: {studentId}", command.Id);
                return Result.Failure<GetStudentResponse>(HttpStatusCode.NotFound,
                    CommonErrors.NotFound<Student>("Id"));
            }
            _unitOfWork.GetRepository<Student,Guid>().Delete(studentExist);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("End DeleteStudentByIdHandler)");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in DeleteStudentByIdHandler {@Message}", ex.Message);
            return Result.Failure<GetStudentResponse>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}