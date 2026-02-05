using FluentValidation;

namespace EngConnect.Application.UseCases.Users.ChangePassword;

public class ChangePasswordCommandValidator: AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId không được để trống.");

        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("Mật khẩu cũ không được để trống.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Mat khẩu mới không được để trống.")
            .MinimumLength(6).WithMessage("Mật khẩu mới phải có ít nhất 6 ký tự.");
    }
    
}