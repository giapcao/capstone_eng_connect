using System.Net;
using EngConnect.Application.UseCases.Students.Common;
using EngConnect.Application.UseCases.Users.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Students.GetStudentById;

public class GetStudentByIdQueryHandler : IQueryHandler<GetStudentByIdQuery, GetStudentResponse>
{
    private readonly ILogger<GetStudentByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAwsStorageService _awsStorageService;

    public GetStudentByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetStudentByIdQueryHandler> logger,
        IAwsStorageService awsStorageService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _awsStorageService = awsStorageService;
    }
    
    public async Task<Result<GetStudentResponse>> HandleAsync(GetStudentByIdQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetStudentByIdQueryHandler : {@query}", query);
        try
        {
            var student = await _unitOfWork.GetRepository<Student, Guid>()
                .FindAll(x => x.Id == query.Id)
                .Include(x => x.User)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (student == null)
            {
                _logger.LogWarning("studentId không tồn tại {studentId}", query.Id);
                return Result.Failure<GetStudentResponse>(HttpStatusCode.NotFound,
                    CommonErrors.NotFound<Student>("Id"));
            }

            var response = new GetStudentResponse
            {
                Id = student.Id,
                UserId = student.UserId,
                Notes = student.Notes,
                School = student.School,
                Grade = student.Grade,
                Class = student.Class,
                Avatar = _awsStorageService.GetFileUrl(student.Avatar, cancellationToken),
                Tags = student.Tags,
                Status = student.Status,
                CreatedAt = student.CreatedAt,
                UpdatedAt = student.UpdatedAt,
                User = new UserInfo
                {
                    FirstName = student.User.FirstName,
                    LastName = student.User.LastName,
                    UserName = student.User.UserName,
                    Email = student.User.Email,
                    Phone = student.User.Phone,
                }
            };

            _logger.LogInformation("End GetStudentByIdQueryHandler)");
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetStudentByIdQueryHandler {@Message}", ex.Message);
            return Result.Failure<GetStudentResponse>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
