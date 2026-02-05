using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Students.UpdateStatusStudent;

public class UpdateStatusStudentCommandHandler : ICommandHandler<UpdateStatusStudentCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateStatusStudentCommandHandler> _logger;

    public UpdateStatusStudentCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateStatusStudentCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger =  logger;
    }
    public async Task<Result> HandleAsync(UpdateStatusStudentCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateStatusStudentCommand: {@command}",command);
        try
        {
            var studentExist = await _unitOfWork.GetRepository<Student, Guid>()
                .FindByIdAsync(command.Id, cancellationToken: cancellationToken);
            
            if (studentExist == null)
            {
                _logger.LogWarning( " Id không tồn tại {id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<Domain.Persistence.Models.User>("Id"));
            }

            studentExist.Status = studentExist.Status == nameof(StudentStatus.Active) ? 
                nameof(StudentStatus.Inactive) : nameof(StudentStatus.Active);
            
            _unitOfWork.GetRepository<Student, Guid>().Update(studentExist);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("End UpdateStatusStudentCommand");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateStatusStudentCommand {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}