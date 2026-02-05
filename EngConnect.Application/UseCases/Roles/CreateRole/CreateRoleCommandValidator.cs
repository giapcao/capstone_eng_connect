using FluentValidation;

namespace EngConnect.Application.UseCases.Roles.CreateRole;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Mã vai trò không được để trống")
            .MaximumLength(50).WithMessage("Mã vai trò không được vượt quá 50 ký tự");
        
        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Mô tả không được vượt quá 255 ký tự");
    }
}
