using System.Net;
using EngConnect.Application.UseCases.Students.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Students.GetStudentById;

public class GetStudentByIdQueryHandler : IQueryHandler<GetStudentByIdQuery,GetStudentResponse>
{
    private readonly ILogger<GetStudentByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetStudentByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetStudentByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<Result<GetStudentResponse>> HandleAsync(GetStudentByIdQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetStudentByIdQueryHandler : {@query}",query);
        try
        {
            var student = await _unitOfWork.GetRepository<Student, Guid>()
                .FindByIdAsync(query.Id, cancellationToken: cancellationToken);
            
            if (student == null)
            {
                _logger.LogWarning( "studentId không tồn tại {studentId}", query.Id);
                return Result.Failure<GetStudentResponse>(HttpStatusCode.NotFound,
                    CommonErrors.NotFound<Student>("Id"));
            }
            
            var existed = student.Adapt<GetStudentResponse>();

            _logger.LogInformation("End GetStudentByIdQueryHandler)");
            return Result.Success(existed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetStudentByIdQueryHandler {@Message}", ex.Message);
            return Result.Failure<GetStudentResponse>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}