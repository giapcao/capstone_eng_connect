using FluentValidation;

namespace EngConnect.BuildingBlock.Application.Validation.Validation;

public static class VietnamesePhoneValidator
{
    public static void VietnamesePhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        ruleBuilder
            .NotEmpty().WithMessage("Số điện thoại không được để trống")
            .Matches(@"(84|0[3|5|7|8|9])+([0-9]{8})\b").WithMessage("Số điện thoại không hợp lệ");
    }
}