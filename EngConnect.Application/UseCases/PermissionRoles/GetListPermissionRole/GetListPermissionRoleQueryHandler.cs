using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.PermissionRoles.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.PermissionRoles.GetListPermissionRole;

public class GetListPermissionRoleQueryHandler : IQueryHandler<GetListPermissionRoleQuery, PaginationResult<GetPermissionRoleResponse>>
{
    private readonly ILogger<GetListPermissionRoleQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetListPermissionRoleQueryHandler(ILogger<GetListPermissionRoleQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginationResult<GetPermissionRoleResponse>>> HandleAsync(GetListPermissionRoleQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListPermissionRoleQueryHandler {@Query}", query);
        try
        {
            var permissionRoleRepo = _unitOfWork.GetRepository<PermissionRole, Guid>();

            var permissionRoles = permissionRoleRepo.FindAll();

            Expression<Func<PermissionRole, bool>>? predicate = x => true;

            
            // Apply filters
            if (query.PermissionId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.PermissionId == query.PermissionId.Value);
            }

            if (query.RoleId.HasValue)
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.RoleId == query.RoleId.Value);
            }
            
            permissionRoles = permissionRoles.Where(predicate);
            
            // Apply search and sort
            permissionRoles = permissionRoles.ApplySearch(query.GetSearchParams(),
                    x => x.PermissionId.ToString(),
                    x => x.RoleId.ToString())
                .ApplySorting(query.GetSortParams());
            
            //Map to response
            var result =
                await permissionRoles.ProjectToPaginatedListAsync<PermissionRole, GetPermissionRoleResponse>(
                    query.GetPaginationParams());


            _logger.LogInformation("End GetListPermissionRoleQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListPermissionRoleQueryHandler: {Message}", ex.Message);
            return Result.Failure<PaginationResult<GetPermissionRoleResponse>>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
