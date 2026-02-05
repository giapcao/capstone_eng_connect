using System.Net;
using EngConnect.Application.UseCases.Roles.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Roles.GetRoleById;

public class GetRoleByIdQueryHandler : IQueryHandler<GetRoleByIdQuery, GetRoleResponse>
{
    private readonly ILogger<GetRoleByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRoleByIdQueryHandler(ILogger<GetRoleByIdQueryHandler> logger, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<GetRoleResponse>> HandleAsync(GetRoleByIdQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetRoleByIdQueryHandler {@Query}", query);
        try
        {
            var roleRepo = _unitOfWork.GetRepository<Role, Guid>();

            var role = await roleRepo.FindFirstAsync(
                    x => x.Id == query.Id,
                    cancellationToken: cancellationToken);
            
            if (role == null)
            {
                _logger.LogWarning("Role not found with ID: {Id}", query.Id);
                return Result.Failure<GetRoleResponse>(HttpStatusCode.BadRequest, RoleErrors.RoleNotFound());
            }
            
            //Map to response
            var result = _mapper.Map<GetRoleResponse>(role);
            
            _logger.LogInformation("End GetRoleByIdQueryHandler");
            return Result.Success(result);
        }catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetRoleByIdQueryHandler: {Message}", ex.Message);
            return Result.Failure<GetRoleResponse>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
