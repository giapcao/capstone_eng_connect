using System.Net;
using EngConnect.BuildingBlock.Application.Base;
using EngConnect.BuildingBlock.Contracts.Abstraction;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using EngConnect.Domain.DomainErrors;
using EngConnect.Domain.Persistence.Models;
using Microsoft.Extensions.Logging;

namespace EngConnect.Application.UseCases.Roles.CreateRole;

public class CreateRoleCommandHandler : ICommandHandler<CreateRoleCommand>
{
    private readonly ILogger<CreateRoleCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoleCommandHandler(ILogger<CreateRoleCommandHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(CreateRoleCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Start CreateRoleCommandHandler {@Command}", command);
        try
        {
            var roleRepo = _unitOfWork.GetRepository<Role, Guid>();

            // Check if role code already exists
            var roleExists = await roleRepo.AnyAsync(x => x.Code == command.Code, cancellationToken);
            if (roleExists)
            {
                _logger.LogWarning("Role already exists with code: {Code}", command.Code);
                return Result.Failure(HttpStatusCode.BadRequest, RoleErrors.RoleAlreadyExists());
            }

            var role = new Role
            {
                Code = command.Code,
                Description = command.Description
            };

            roleRepo.Add(role);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("End CreateRoleCommandHandler");
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred in CreateRoleCommandHandler: {Message}", ex.Message);
            return Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());
        }
    }
}
