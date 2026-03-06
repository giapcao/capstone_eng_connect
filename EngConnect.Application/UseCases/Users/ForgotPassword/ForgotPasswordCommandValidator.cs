using FluentValidation;

namespace EngConnect.Application.UseCases.Users.ForgotPassword;

public class ForgotPasswordCommandValidator: AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty().WithMessage("Email không được để trống");
    }
}