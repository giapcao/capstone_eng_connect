using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.User.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.User.GetListUser;

public class GetListUserQueryHandler : IQueryHandler<GetListUserQuery, PaginationResult<GetUserResponse>>
{
    private readonly ILogger<GetListUserQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetListUserQueryHandler(ILogger<GetListUserQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginationResult<GetUserResponse>>> HandleAsync(GetListUserQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListUserQueryHandler: {@Query}", query);
        try
        {
            var users = _unitOfWork.GetRepository<Domain.Persistence.Models.User, Guid>().FindAll();

            Expression<Func<Domain.Persistence.Models.User, bool>>? predicate = x => true;
            
            // Validate status filter
            if (ValidationUtil.IsNotNullOrEmpty(query.Status))
            {
                predicate = predicate.CombineAndAlsoExpressions(x => query.Status.Contains(x.Status));
            }
            
            users = users.Where(predicate);

            // Apply search and sort
            users = users.ApplySearch(query.GetSearchParams(),
                    x => x.FirstName ?? string.Empty,
                    x => x.LastName ?? string.Empty,
                    x => x.Email ?? string.Empty,
                    x => x.Phone ?? string.Empty,
                    x => x.UserName ?? string.Empty)
                .ApplySorting(query.GetSortParams());

            // Map to GetUserResponse
            var result =
                await users.ProjectToPaginatedListAsync<Domain.Persistence.Models.User, GetUserResponse>(
                    query.GetPaginationParams());

            _logger.LogInformation("End GetListUserQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListUserQueryHandler {@Message}", ex.Message);
            return Result.Failure<PaginationResult<GetUserResponse>>(HttpStatusCode.InternalServerError,
                CommonErrors.InternalServerError());
        }
    }
}

