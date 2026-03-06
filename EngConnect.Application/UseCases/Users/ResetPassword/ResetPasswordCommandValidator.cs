using FluentValidation;

namespace EngConnect.Application.UseCases.Users.ResetPassword;

public class ResetPasswordCommandValidator: AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token không được để trống");
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Mật khẩu mới không được để trống")
            .MinimumLength(6).WithMessage("Mật khẩu mới phải có ít nhất 6 ký tự");
    }
}