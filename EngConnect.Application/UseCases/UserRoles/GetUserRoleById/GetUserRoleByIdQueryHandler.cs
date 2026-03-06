using System.Net;
using EngConnect.Application.UseCases.UserRoles.Common;
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

namespace EngConnect.Application.UseCases.UserRoles.GetUserRoleById;

public class GetUserRoleByIdQueryHandler : IQueryHandler<GetUserRoleByIdQuery, GetUserRoleResponse>
{
    private readonly ILogger<GetUserRoleByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetUserRoleByIdQueryHandler(ILogger<GetUserRoleByIdQueryHandler> logger, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<GetUserRoleResponse>> HandleAsync(GetUserRoleByIdQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetUserRoleByIdQueryHandler {@Query}", query);
        try
        {
            var userRoleRepo = _unitOfWork.GetRepository<UserRole, Guid>();

            var userRole = await userRoleRepo.FindAll(
                    x => x.Id == query.Id,
                    tracking: false)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (userRole == null)
            {
                _logger.LogWarning("UserRole not found with ID: {Id}", query.Id);
                return Result.Failure<GetUserRoleResponse>(HttpStatusCode.BadRequest, UserRoleErrors.UserRoleNotFound());
            }

            //Map to response
            var result = _mapper.Map<GetUserRoleResponse>(userRole);
            
            _logger.LogInformation("End GetUserRoleByIdQueryHandler");
            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetUserRoleByIdQueryHandler: {Message}", ex.Message);
            return Result.Failure<GetUserRoleResponse>(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
