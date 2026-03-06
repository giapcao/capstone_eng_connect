using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.UserRoles.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.UserRoles.GetListUserRole;

public class GetListUserRoleQueryHandler : IQueryHandler<GetListUserRoleQuery, PaginationResult<GetUserRoleResponse>>
{
    private readonly ILogger<GetListUserRoleQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetListUserRoleQueryHandler(ILogger<GetListUserRoleQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginationResult<GetUserRoleResponse>>> HandleAsync(GetListUserRoleQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListUserRoleQueryHandler {@Query}", query);
        try
        {
            var userRoleRepo = _unitOfWork.GetRepository<UserRole, Guid>();

            var userRoles = userRoleRepo.FindAll();

            // Apply filters
            
            Expression<Func<UserRole, bool>>? predicate = x => true;
            
            if (query.UserId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.UserId == query.UserId.Value);
            }

            if (query.RoleId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.RoleId == query.RoleId.Value);
            }
            
            userRoles = userRoles.Where(predicate);
            
            // Apply search and sort
            userRoles = userRoles.ApplySearch(query.GetSearchParams(),
                    x => x.UserId.ToString(),
                    x => x.RoleId.ToString())
                .ApplySorting(query.GetSortParams());
            
            //Map to response
            var result =
                await userRoles.ProjectToPaginatedListAsync<UserRole, GetUserRoleResponse>(
                    query.GetPaginationParams());


            _logger.LogInformation("End GetListUserRoleQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListUserRoleQueryHandler: {Message}", ex.Message);
            return Result.Failure<PaginationResult<GetUserRoleResponse>>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
