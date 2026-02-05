using FluentValidation;

namespace EngConnect.Application.UseCases.Authentication.LoginByUser;

public class LoginByUserCommandValidator: AbstractValidator<LoginByUserCommand>
{
    public LoginByUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email không được để trống.")
            .MaximumLength(100).WithMessage("Email không được vượt quá 100 ký tự.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Mật khẩu không được để trống.")
            .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự.")
            .MaximumLength(100).WithMessage("Mật khẩu không được vượt quá 100 ký tự.");
    }
}