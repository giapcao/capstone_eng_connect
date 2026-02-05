using FluentValidation;

namespace EngConnect.Application.UseCases.Authentication.VerifyEmail;

public class VerifyEmailCommandValidator : AbstractValidator<VerifyEmailCommand>
{
    public VerifyEmailCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token không được để trống");
    }
}