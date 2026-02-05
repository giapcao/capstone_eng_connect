using System.Linq.Expressions;
using System.Net;
using EngConnect.Application.UseCases.Roles.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Application.Utils;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Roles.GetListRole;

public class GetListRoleQueryHandler : IQueryHandler<GetListRoleQuery, PaginationResult<GetRoleResponse>>
{
    private readonly ILogger<GetListRoleQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetListRoleQueryHandler(ILogger<GetListRoleQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PaginationResult<GetRoleResponse>>> HandleAsync(GetListRoleQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetListRoleQueryHandler {@Query}", query);
        try
        {
            var roles = _unitOfWork.GetRepository<Role, Guid>()
                .FindAll();

            Expression<Func<Role, bool>>? predicate = x => true;
            
            // Apply filters
            if (ValidationUtil.IsNotNullOrEmpty(query.Code))
            {
                predicate = predicate.CombineAndAlsoExpressions(x => x.Code.Contains(query.Code));

            }
            
            roles = roles.Where(predicate);
            
            // Apply search and sort
            roles = roles.ApplySearch(query.GetSearchParams(), 
                    x => x.Code,
                    x => x.Description)
                .ApplySorting(query.GetSortParams());
            // Map to GetRoleResponse
            var result =
                await roles.ProjectToPaginatedListAsync<Role,GetRoleResponse>(
                    query.GetPaginationParams());

            _logger.LogInformation("End GetListRoleQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetListRoleQueryHandler: {Message}", ex.Message);
            return Result.Failure<PaginationResult<GetRoleResponse>>(
                HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
