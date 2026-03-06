using System.Net;
using EngConnect.Application.UseCases.Users.Common;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Users.GetUserById;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, GetUserResponseDetail>
{
    private readonly ILogger<GetUserByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public GetUserByIdQueryHandler(ILogger<GetUserByIdQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetUserResponseDetail>> HandleAsync(GetUserByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start GetUserByIdQueryHandler: {@Query}", query);
        try
        {
            var userRepository = _unitOfWork.GetRepository<User, Guid>();
            var user = await userRepository.FindAll(x => x.Id == query.Id)
                .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                        .ThenInclude(x => x.PermissionRoles)
                            .ThenInclude(x => x.Permission)
                .ProjectToType<GetUserResponseDetail>()
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", query.Id);
                return Result.Failure<GetUserResponseDetail>(
                    HttpStatusCode.BadRequest, UserErrors.UserNotFound());
            }

            _logger.LogInformation("End GetUserByIdQueryHandler");
            return Result.Success(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in GetUserByIdQueryHandler {@Message}", ex.Message);
            return Result.Failure<GetUserResponseDetail>(HttpStatusCode.InternalServerError, 
            CommonErrors.InternalServerError());
        }
    }
}

