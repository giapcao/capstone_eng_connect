using System.Net;
using EngConnect.Application.UseCases.CourseResources.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseResources.GetCourseResourceById;

public class GetCourseResourceByIdQueryHandler : IQueryHandler<GetCourseResourceByIdQuery, GetCourseResourceResponse>
{
    private readonly ILogger<GetCourseResourceByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCourseResourceByIdQueryHandler(ILogger<GetCourseResourceByIdQueryHandler> logger, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<GetCourseResourceResponse>> HandleAsync(GetCourseResourceByIdQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetCourseResourceByIdQueryHandler {@Query}", query);
        try
        {
            var courseResourceRepo = _unitOfWork.GetRepository<CourseResource, Guid>();

            var courseResource = await courseResourceRepo.FindSingleAsync(
                x => x.Id == query.Id,
                tracking: false,
                cancellationToken: cancellationToken);

            if (courseResource == null)
            {
                _logger.LogWarning("CourseResource not found with ID: {Id}", query.Id);
                return Result.Failure<GetCourseResourceResponse>(HttpStatusCode.NotFound, new Error("CourseResourceNotFound", "Tài nguyên không tồn tại"));
            }

            //Map to response
            var result = _mapper.Map<GetCourseResourceResponse>(courseResource);
            
            _logger.LogInformation("End GetCourseResourceByIdQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetCourseResourceByIdQueryHandler: {Message}", ex.Message);
            return Result.Failure<GetCourseResourceResponse>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
