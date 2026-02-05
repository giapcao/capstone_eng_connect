using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.UserRoles.DeleteUserRole;

public class DeleteUserRoleCommandHandler : ICommandHandler<DeleteUserRoleCommand>
{
    private readonly ILogger<DeleteUserRoleCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserRoleCommandHandler(ILogger<DeleteUserRoleCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(DeleteUserRoleCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start DeleteUserRoleCommandHandler {@Command}", command);
        try
        {
            var userRoleRepo = _unitOfWork.GetRepository<UserRole, Guid>();

            var userRole = await userRoleRepo.FindSingleAsync(
                x=> x.Id == command.Id, 
                cancellationToken:cancellationToken);
            if (userRole == null)
            {
                _logger.LogWarning("UserRole not found with ID: {Id}", command.Id);
                return Result.Failure(HttpStatusCode.NotFound, UserRoleErrors.UserRoleNotFound());
            }

            userRoleRepo.Delete(userRole);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End DeleteUserRoleCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in DeleteUserRoleCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
