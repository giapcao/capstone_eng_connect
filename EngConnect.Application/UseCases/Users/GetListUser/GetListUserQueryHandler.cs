using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.Users.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Users.GetListUser;

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
            var users = _unitOfWork.GetRepository<User, Guid>().FindAll();

            Expression<Func<User, bool>>? predicate = x => true;
            
            // Validate status filter
            if (ValidationUtil.IsNotNullOrEmpty(query.Status))
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.Status.Contains(query.Status));
            }
            
            users = users.Where(predicate);

            // Apply search and sort
            users = users.ApplySearch(query.GetSearchParams(),
                    x => x.FirstName,
                    x => x.LastName,
                    x => x.Email,
                    x => x.Phone ?? string.Empty,
                    x => x.UserName)
                .ApplySorting(query.GetSortParams());

            //Implement flag to show user roles and permissions
            
            // Map to GetUserResponse
            var result =
                await users.ProjectToPaginatedListAsync<User, GetUserResponse>(
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

