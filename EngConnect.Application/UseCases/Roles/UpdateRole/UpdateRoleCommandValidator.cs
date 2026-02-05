using FluentValidation;

namespace EngConnect.Application.UseCases.Roles.UpdateRole;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id không được để trống");
        
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Mã vai trò không được để trống")
            .MaximumLength(50).WithMessage("Mã vai trò không được vượt quá 50 ký tự");
        
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Mô tả không được vượt quá 500 ký tự");
    }
}
