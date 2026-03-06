using FluentValidation;

namespace EngConnect.Application.UseCases.Users.CreateUser;

public class CreateUserCommandValidator: AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email không được để trống");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Mật khẩu không được để trống")
            .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự");
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Tên không được để trống");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Họ không được để trống");
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username không được để trống");
    }
    
}