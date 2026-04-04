using System.Net;
using EngConnect.Application.UseCases.CourseModules.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseModules.GetCourseModuleById;

public class GetCourseModuleByIdQueryHandler : IQueryHandler<GetCourseModuleByIdQuery, GetCourseModuleDetailResponse>
{
    private readonly ILogger<GetCourseModuleByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCourseModuleByIdQueryHandler(ILogger<GetCourseModuleByIdQueryHandler> logger, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<GetCourseModuleDetailResponse>> HandleAsync(GetCourseModuleByIdQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetCourseModuleByIdQueryHandler {@Query}", query);
        try
        {
            var courseModuleRepo = _unitOfWork.GetRepository<CourseModule, Guid>();

            var courseModule = await courseModuleRepo.FindAll(
                x => x.Id == query.Id,
                cancellationToken: cancellationToken)
                .Include(x => x.CourseModuleCourseSessions)
                .ThenInclude(x => x.CourseSession)
                .FirstOrDefaultAsync(cancellationToken);

            if (courseModule == null)
            {
                _logger.LogWarning("CourseModule not found with ID: {Id}", query.Id);
                return Result.Failure<GetCourseModuleDetailResponse>(HttpStatusCode.NotFound, new Error("CourseModuleNotFound", "Module không tồn tại"));
            }

            //Map to response
            var result = _mapper.Map<GetCourseModuleDetailResponse>(courseModule);
            result.ParentModuleId = courseModule.ParentModuleId;
            
            _logger.LogInformation("End GetCourseModuleByIdQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetCourseModuleByIdQueryHandler: {Message}", ex.Message);
            return Result.Failure<GetCourseModuleDetailResponse>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
