using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Students.UpdateStudent;

public class UpdateStudentCommandHandler : ICommandHandler<UpdateStudentCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateStudentCommandHandler> _logger;

    public UpdateStudentCommandHandler(IUnitOfWork unitOfWork,ILogger<UpdateStudentCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger =  logger;
    }
    
    
    public async Task<Result> HandleAsync(UpdateStudentCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start UpdateStudentCommandHandler: {@command}",command);
        try
        {
            var studentExist = await _unitOfWork.GetRepository<Student, Guid>()
                .AnyAsync(x => x.Id == command.Id && x.UserId == command.UserId, cancellationToken);
            
            if (!studentExist)
            {
                _logger.LogWarning( " Id không tồn tại {id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<Domain.Persistence.Models.User>("Id"));
            }

            var studentEntity = command.Adapt<Student>();
            
            _unitOfWork.GetRepository<Student, Guid>().Update(studentEntity);
            await _unitOfWork.SaveChangesAsync();
            
            _logger.LogInformation("End UpdateStudentCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in UpdateStudentCommandHandler {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
