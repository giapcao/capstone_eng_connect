using EngConnect.BuildingBlock.Contracts.Shared.Utils;
using FluentValidation;

namespace EngConnect.BuildingBlock.Application.Validation.Validation;

public static class EnumValidator
{
    /// <summary>
    /// The value must be provided and must be a valid enum value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string> ValidEnum<T, TEnum>(this IRuleBuilder<T, string> ruleBuilder)
        where TEnum : struct, Enum
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Trạng thái không được để trống")
            .Must(value => Enum.TryParse<TEnum>(value, true, out _))
            .WithMessage(
                $"Trạng thái không hợp lệ. Giá trị hợp lệ là: {string.Join(", ", Enum.GetNames<TEnum>())}");
    }

    /// <summary>
    /// If the value is provided (not null or empty), it must be a valid enum value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string?> ValidNullableEnum<T, TEnum>(this IRuleBuilder<T, string?> ruleBuilder)
        where TEnum : struct, Enum
    {
        return ruleBuilder
            // Allow null or empty
            .Must(value => ValidationUtil.IsNullOrEmpty(value) || Enum.TryParse<TEnum>(value, true, out _))
            .WithMessage(
                $"Trạng thái không hợp lệ. Giá trị hợp lệ là: {string.Join(", ", Enum.GetNames<TEnum>())}");
    }

    /// <summary>
    /// If the values are provided (not null or empty), they must be valid enum values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <returns></returns>
    public static IRuleBuilderOptions<T, string[]> ValidEnumArray<T, TEnum>(this IRuleBuilder<T, string[]> ruleBuilder)
        where TEnum : struct, Enum
    {
        return ruleBuilder
            // Allow null or empty
            .Must(values => ValidationUtil.IsNullOrEmpty(values) || values.All(enumValue => Enum.TryParse<TEnum>(enumValue, true, out _)))
            .WithMessage(
                $"Trạng thái không hợp lệ. Giá trị hợp lệ là: {string.Join(", ", Enum.GetNames<TEnum>())}");
    }
}