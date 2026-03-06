using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.Permissions.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Permissions.GetListPermission;

public class GetListPermissionQueryHandler : IQueryHandler<GetListPermissionQuery, PaginationResult<GetPermissionResponse>>
{
    private readonly ILogger<GetListPermissionQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetListPermissionQueryHandler(ILogger<GetListPermissionQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginationResult<GetPermissionResponse>>> HandleAsync(GetListPermissionQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListPermissionQueryHandler {@Query}", query);
        try
        {
            var permissionRepo = _unitOfWork.GetRepository<Permission, Guid>();

            var permissions = permissionRepo.FindAll();

            // Apply filters
            
            Expression<Func<Permission, bool>>? predicate = x => true;
            
            if (ValidationUtil.IsNotNullOrEmpty(query.Code))
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.Code.Contains(query.Code));
            }

            permissions = permissions.Where(predicate);
            
            //Apply search and sort
            // Apply search and sort
            permissions = permissions.ApplySearch(query.GetSearchParams(), 
                    x => x.Code,
                    x => x.Description)
                .ApplySorting(query.GetSortParams());

            //Map to response
            var result =
                await permissions.ProjectToPaginatedListAsync<Permission,GetPermissionResponse>(
                    query.GetPaginationParams());

            _logger.LogInformation("End GetListPermissionQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListPermissionQueryHandler: {Message}", ex.Message);
            return Result.Failure<PaginationResult<GetPermissionResponse>>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
