using FluentValidation;

namespace EngConnect.Application.UseCases.Permissions.CreatePermission;

public class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand>
{
    public CreatePermissionCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Mã quyền không được để trống")
            .MaximumLength(50).WithMessage("Mã quyền không được vượt quá 50 ký tự");
        
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Mô tả không được vượt quá 500 ký tự");
    }
}
