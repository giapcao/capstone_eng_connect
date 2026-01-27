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
            
            var userExists = await _unitOfWork.GetRepository<Domain.Persistence.Models.User, Guid>()
                .AnyAsync(x=>x.Id == command.UserId, cancellationToken: cancellationToken);
            
            var studentExists =await  _unitOfWork.GetRepository<Student, Guid>()
                .AnyAsync(x=>x.UserId == command.UserId, cancellationToken);
            
            if (!userExists)
            {
                _logger.LogWarning( " UserId không tồn tại : {userId}", command.UserId);
                return Result.Failure<Guid>(HttpStatusCode.NotFound, CommonErrors.NotFound<Domain.Persistence.Models.User>("UserId không tồn tại"));
            }
            
            if (studentExists)
            {
                _logger.LogWarning( " Student đã tồn tại: {userId}", command.UserId);
                return Result.Failure<Guid>(HttpStatusCode.BadRequest, CommonErrors.AlreadyExists("StudentId","đã tồn tại"));
            }
            
            var student = command.Adapt<Student>();
            student.Status = nameof(StudentStatus.Active);
            _unitOfWork.GetRepository<Student, Guid>().Add(student);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("End CreateStudentCommandHandler");
            
            return Result.Success(student);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateStudentCommandHandler {@Message}", ex.Message);
            return Result.Failure<Guid>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}