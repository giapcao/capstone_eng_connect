using System.Net;
using EngConnect.Application.UseCases.PermissionRoles.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.PermissionRoles.GetPermissionRoleById;

public class GetPermissionRoleByIdQueryHandler : IQueryHandler<GetPermissionRoleByIdQuery, GetPermissionRoleResponse>
{
    private readonly ILogger<GetPermissionRoleByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPermissionRoleByIdQueryHandler(ILogger<GetPermissionRoleByIdQueryHandler> logger, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<GetPermissionRoleResponse>> HandleAsync(GetPermissionRoleByIdQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetPermissionRoleByIdQueryHandler {@Query}", query);
        try
        {
            var permissionRoleRepo = _unitOfWork.GetRepository<PermissionRole, Guid>();

            var permissionRole = await permissionRoleRepo.FindAll(
                    x => x.Id == query.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (permissionRole == null)
            {
                _logger.LogWarning("PermissionRole not found with ID: {Id}", query.Id);
                return Result.Failure<GetPermissionRoleResponse>(HttpStatusCode.NotFound, PermissionRoleErrors.PermissionRoleNotFound());
            }

            var result = _mapper.Map<GetPermissionRoleResponse>(permissionRole);
            
            _logger.LogInformation("End GetPermissionRoleByIdQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetPermissionRoleByIdQueryHandler: {Message}", ex.Message);
            return Result.Failure<GetPermissionRoleResponse>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
