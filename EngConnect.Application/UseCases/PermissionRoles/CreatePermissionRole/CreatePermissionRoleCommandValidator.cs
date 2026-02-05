using FluentValidation;

namespace EngConnect.Application.UseCases.PermissionRoles.CreatePermissionRole;

public class CreatePermissionRoleCommandValidator : AbstractValidator<CreatePermissionRoleCommand>
{
    public CreatePermissionRoleCommandValidator()
    {
        RuleFor(x => x.PermissionId)
            .NotEmpty().WithMessage("PermissionId không được để trống");
        
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("RoleId không được để trống");
    }
}
