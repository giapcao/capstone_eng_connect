using System.Net;
using EngConnect.Application.UseCases.Permissions.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Permissions.GetPermissionById;

public class GetPermissionByIdQueryHandler : IQueryHandler<GetPermissionByIdQuery, GetPermissionResponse>
{
    private readonly ILogger<GetPermissionByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPermissionByIdQueryHandler(ILogger<GetPermissionByIdQueryHandler> logger, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<GetPermissionResponse>> HandleAsync(GetPermissionByIdQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetPermissionByIdQueryHandler {@Query}", query);
        try
        {
            var permissionRepo = _unitOfWork.GetRepository<Permission, Guid>();

            var permission = await permissionRepo.FindSingleAsync(
                x => x.Id == query.Id,
                tracking: false,
                cancellationToken: cancellationToken);
            
            if (ValidationUtil.IsNullOrEmpty(permission))
            {
                _logger.LogWarning("Permission not found with ID: {Id}", query.Id);
                return Result.Failure<GetPermissionResponse>(HttpStatusCode.NotFound, PermissionErrors.PermissionNotFound());
            }
            
            //Map to response
            var result =  _mapper.Map<GetPermissionResponse>(permission);

            _logger.LogInformation("End GetPermissionByIdQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetPermissionByIdQueryHandler: {Message}", ex.Message);
            return Result.Failure<GetPermissionResponse>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
