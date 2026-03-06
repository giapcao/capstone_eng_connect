using FluentValidation;

namespace EngConnect.Application.UseCases.Authentication.RegisterUser;

public class RegisterUserCommandValidator: AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Tên không được để trống.")
            .MaximumLength(50).WithMessage("Tên không được vượt quá 50 ký tự.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Họ không được để trống.")
            .MaximumLength(50).WithMessage("Họ không được vượt quá 50 ký tự.");

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username không được để trống.")
            .MaximumLength(30).WithMessage("Username không được vượt quá 30 ký tự.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email không được để trống.")
            .EmailAddress().WithMessage("Email không hợp lệ.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Mật khẩu không được để trống.")
            .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự.");

        RuleFor(x => x.Phone)
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Số điện thoại không hợp lệ.");

        RuleFor(x => x.AddressNum)
            .MaximumLength(100).WithMessage("Address number must not exceed 100 characters.");
    }
}