using FluentValidation;

namespace EngConnect.BuildingBlock.Application.Validation.Validation;

public static class ValidatorExtensions
{
    public static void StrongPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        ruleBuilder
            .NotEmpty().WithMessage("Mật khẩu không được để trống")
            .MinimumLength(8).WithMessage("Mật khẩu phải có ít nhất 8 ký tự")
            .Matches("[A-Z]").WithMessage(" Mật khẩu phải có ít nhất 1 ký tự viết hoa")
            .Matches("[a-z]").WithMessage("Mật khẩu phải có ít nhất 1 ký tự viết thường")
            .Matches("[0-9]").WithMessage("Mật khẩu phải có ít nhất 1 chữ số")
            .Matches("[^a-zA-Z0-9]").WithMessage("Mật khẩu phải có ít nhất 1 ký tự đặc biệt");
    }
}