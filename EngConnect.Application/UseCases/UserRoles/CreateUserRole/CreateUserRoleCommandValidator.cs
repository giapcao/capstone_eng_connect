using FluentValidation;

namespace EngConnect.Application.UseCases.UserRoles.CreateUserRole;

public class CreateUserRoleCommandValidator : AbstractValidator<CreateUserRoleCommand>
{
    public CreateUserRoleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId không được để trống");
        
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("RoleId không được để trống");
    }
}
