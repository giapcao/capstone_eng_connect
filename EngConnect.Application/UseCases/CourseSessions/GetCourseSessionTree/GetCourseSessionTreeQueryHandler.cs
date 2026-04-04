using System.Net;
using EngConnect.Application.UseCases.CourseSessions.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseSessions.GetCourseSessionTree;

public class GetCourseSessionTreeQueryHandler : IQueryHandler<GetCourseSessionTreeQuery, GetCourseSessionTreeResponse>
{
    private readonly ILogger<GetCourseSessionTreeQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetCourseSessionTreeQueryHandler(ILogger<GetCourseSessionTreeQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetCourseSessionTreeResponse>> HandleAsync(GetCourseSessionTreeQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetCourseSessionTreeQueryHandler {@Query}", query);
        try
        {
            var courseSessionRepo = _unitOfWork.GetRepository<CourseSession, Guid>();
            var startSession = await courseSessionRepo.FindSingleAsync(x => x.Id == query.Id, tracking: false,
                cancellationToken: cancellationToken);

            if (startSession == null)
            {
                _logger.LogWarning("CourseSession not found with ID: {Id}", query.Id);
                return Result.Failure<GetCourseSessionTreeResponse>(HttpStatusCode.NotFound,
                    new Error("CourseSessionNotFound", "Session không tồn tại"));
            }

            var visited = new HashSet<Guid>();
            var current = startSession;
            var chain = new List<GetCourseSessionTreeNodeResponse>();

            while (current != null)
            {
                if (!visited.Add(current.Id))
                {
                    break;
                }

                chain.Add(MapNode(current));

                if (!current.ParentSessionId.HasValue)
                {
                    break;
                }

                current = await courseSessionRepo.FindSingleAsync(x => x.Id == current.ParentSessionId.Value,
                    tracking: false,
                    cancellationToken: cancellationToken);
            }

            var response = new GetCourseSessionTreeResponse
            {
                Session = chain[0],
                ParentChain = chain.Skip(1).ToList()
            };

            _logger.LogInformation("End GetCourseSessionTreeQueryHandler");
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetCourseSessionTreeQueryHandler: {Message}", ex.Message);
            return Result.Failure<GetCourseSessionTreeResponse>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }

    private static GetCourseSessionTreeNodeResponse MapNode(CourseSession session)
    {
        return new GetCourseSessionTreeNodeResponse
        {
            Id = session.Id,
            ParentSessionId = session.ParentSessionId,
            Title = session.Title,
            Description = session.Description,
            Outcomes = session.Outcomes,
            CreatedAt = session.CreatedAt,
            UpdatedAt = session.UpdatedAt
        };
    }
}
