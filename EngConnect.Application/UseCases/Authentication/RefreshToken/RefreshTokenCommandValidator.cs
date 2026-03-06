using FluentValidation;

namespace EngConnect.Application.UseCases.Authentication.RefreshToken;

public class RefreshTokenCommandValidator: AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty().WithMessage("Access token không được để trống");
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh token không được để trống");
    }
}