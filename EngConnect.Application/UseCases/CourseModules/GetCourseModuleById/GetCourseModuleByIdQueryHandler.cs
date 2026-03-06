using System.Net;
using EngConnect.Application.UseCases.CourseModules.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseModules.GetCourseModuleById;

public class GetCourseModuleByIdQueryHandler : IQueryHandler<GetCourseModuleByIdQuery, GetCourseModuleResponse>
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

    public async Task<Result<GetCourseModuleResponse>> HandleAsync(GetCourseModuleByIdQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetCourseModuleByIdQueryHandler {@Query}", query);
        try
        {
            var courseModuleRepo = _unitOfWork.GetRepository<CourseModule, Guid>();

            var courseModule = await courseModuleRepo.FindSingleAsync(
                x => x.Id == query.Id,
                tracking: false,
                cancellationToken: cancellationToken);

            if (courseModule == null)
            {
                _logger.LogWarning("CourseModule not found with ID: {Id}", query.Id);
                return Result.Failure<GetCourseModuleResponse>(HttpStatusCode.NotFound, new Error("CourseModuleNotFound", "Module không tồn tại"));
            }

            //Map to response
            var result = _mapper.Map<GetCourseModuleResponse>(courseModule);
            
            _logger.LogInformation("End GetCourseModuleByIdQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetCourseModuleByIdQueryHandler: {Message}", ex.Message);
            return Result.Failure<GetCourseModuleResponse>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
