using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Students.CreateStudent;

public class CreateStudentCommandHandler : ICommandHandler<CreateStudentCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateStudentCommandHandler> _logger;

    public CreateStudentCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateStudentCommandHandler> logger)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> HandleAsync(CreateStudentCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start CreateStudentCommandHandler: {@command}",command);
        try
        {
            var student = command.Adapt<Student>();
            
            var user = await _unitOfWork.GetRepository<Domain.Persistence.Models.User, Guid>()
                .AnyAsync(x=>x.Id == student.UserId, cancellationToken: cancellationToken);
            
            var studentExist = await  _unitOfWork.GetRepository<Student, Guid>()
                .AnyAsync(x=>x.UserId == student.UserId, cancellationToken);
            if (!user || studentExist)
            {
                _logger.LogWarning( " UserId không tồn tại hoặc student đã tồn tại: {userId}", command.UserId);
                return Result.Failure(HttpStatusCode.NotFound, CommonErrors.NotFound<Domain.Persistence.Models.User>("UserId hoặc Student đã tồn tại"));
            }
            student.Status = nameof(StudentStatus.Active);
            _unitOfWork.GetRepository<Student, Guid>().Add(student);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("End CreateStudentCommandHandler");
            
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateStudentCommandHandler {@Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}