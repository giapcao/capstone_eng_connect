using System.Net;
using EngConnect.Application.UseCases.CourseModules.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.CourseModules.GetCourseModuleTree;

public class GetCourseModuleTreeQueryHandler : IQueryHandler<GetCourseModuleTreeQuery, GetCourseModuleTreeResponse>
{
    private readonly ILogger<GetCourseModuleTreeQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetCourseModuleTreeQueryHandler(ILogger<GetCourseModuleTreeQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetCourseModuleTreeResponse>> HandleAsync(GetCourseModuleTreeQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetCourseModuleTreeQueryHandler {@Query}", query);
        try
        {
            var courseModuleRepo = _unitOfWork.GetRepository<CourseModule, Guid>();
            var startModule = await courseModuleRepo.FindSingleAsync(x => x.Id == query.Id, tracking: false,
                cancellationToken: cancellationToken);

            if (startModule == null)
            {
                _logger.LogWarning("CourseModule not found with ID: {Id}", query.Id);
                return Result.Failure<GetCourseModuleTreeResponse>(HttpStatusCode.NotFound,
                    new Error("CourseModuleNotFound", "Module không tồn tại"));
            }

            var visited = new HashSet<Guid>();
            var current = startModule;
            var chain = new List<GetCourseModuleTreeNodeResponse>();

            while (current != null)
            {
                if (!visited.Add(current.Id))
                {
                    break;
                }

                chain.Add(MapNode(current));

                if (!current.ParentModuleId.HasValue)
                {
                    break;
                }

                current = await courseModuleRepo.FindSingleAsync(x => x.Id == current.ParentModuleId.Value,
                    tracking: false,
                    cancellationToken: cancellationToken);
            }

            var response = new GetCourseModuleTreeResponse
            {
                Module = chain[0],
                ParentChain = chain.Skip(1).ToList()
            };

            _logger.LogInformation("End GetCourseModuleTreeQueryHandler");
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetCourseModuleTreeQueryHandler: {Message}", ex.Message);
            return Result.Failure<GetCourseModuleTreeResponse>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }

    private static GetCourseModuleTreeNodeResponse MapNode(CourseModule module)
    {
        return new GetCourseModuleTreeNodeResponse
        {
            Id = module.Id,
            ParentModuleId = module.ParentModuleId,
            Title = module.Title,
            Description = module.Description,
            Outcomes = module.Outcomes,
            CreatedAt = module.CreatedAt,
            UpdatedAt = module.UpdatedAt
        };
    }
}
