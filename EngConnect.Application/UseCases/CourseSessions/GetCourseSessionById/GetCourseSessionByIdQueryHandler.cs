using System.Net;
using EngConnect.Application.UseCases.CourseSessions.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseSessions.GetCourseSessionById;

public class GetCourseSessionByIdQueryHandler : IQueryHandler<GetCourseSessionByIdQuery, GetCourseSessionResponse>
{
    private readonly ILogger<GetCourseSessionByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCourseSessionByIdQueryHandler(ILogger<GetCourseSessionByIdQueryHandler> logger, IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<GetCourseSessionResponse>> HandleAsync(GetCourseSessionByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetCourseSessionByIdQueryHandler {@Query}", query);
        try
        {
            var courseSessionRepo = _unitOfWork.GetRepository<CourseSession, Guid>();

            var courseSession = await courseSessionRepo.FindSingleAsync(
                x => x.Id == query.Id,
                tracking: false,
                cancellationToken: cancellationToken);

            if (courseSession == null)
            {
                _logger.LogWarning("CourseSession not found with ID: {Id}", query.Id);
                return Result.Failure<GetCourseSessionResponse>(HttpStatusCode.NotFound,
                    new Error("CourseSessionNotFound", "Session không tồn tại"));
            }

            //Map to response
            var result = _mapper.Map<GetCourseSessionResponse>(courseSession);
            result.ParentSessionId = courseSession.ParentSessionId;

            _logger.LogInformation("End GetCourseSessionByIdQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetCourseSessionByIdQueryHandler: {Message}", ex.Message);
            return Result.Failure<GetCourseSessionResponse>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}
