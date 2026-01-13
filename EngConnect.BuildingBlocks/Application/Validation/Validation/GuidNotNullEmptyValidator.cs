using FluentValidation;

namespace EngConnect.BuildingBlock.Application.Validation.Validation;

public static class GuidNotNullEmptyValidator
{
    public static IRuleBuilderOptions<T, Guid> GuidNotNullEmpty<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("ID không được để trống")
            .NotEqual(Guid.Empty).WithMessage("ID không đúng định dạng");
    }
}