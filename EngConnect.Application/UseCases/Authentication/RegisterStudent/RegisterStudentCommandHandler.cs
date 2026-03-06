using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Constants;
using EngConnect.Domain.Persistence.Models;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Authentication.RegisterStudent;

public class RegisterStudentCommandHandler : ICommandHandler<RegisterStudentCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegisterStudentCommandHandler> _logger;
    private readonly IMapper _mapper;

    public RegisterStudentCommandHandler(IUnitOfWork unitOfWork, ILogger<RegisterStudentCommandHandler> logger, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<Result> HandleAsync(RegisterStudentCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start RegisterStudentCommandHandler: {@command}",command);
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
            
            var student = _mapper.Map<Student>(command);
            student.Status = nameof(StudentStatus.Active);
            // Do not include Tags - left as null
            
            _unitOfWork.GetRepository<Student, Guid>().Add(student);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("End RegisterStudentCommandHandler");
            
            return Result.Success(student);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in RegisterStudentCommandHandler {@Message}", ex.Message);
            return Result.Failure<Guid>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
