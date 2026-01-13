using System.Net;

namespace EngConnect.BuildingBlock.Contracts.Shared;

public static class EnumHelper
{
    /// <summary>
    ///     Converts an array of enum values to their underlying integer values
    /// </summary>
    public static int[] ToIntArray<TEnum>(this TEnum[] enumValues) where TEnum : struct, Enum
    {
        return enumValues.Select(e => Convert.ToInt32(e)).ToArray();
    }

    /// <summary>
    ///     Converts an array of enum values to their string representation
    /// </summary>
    /// <param name="enumValues"></param>
    /// <typeparam name="TEnum"></typeparam>
    /// <returns></returns>
    public static string[] ToStringArray<TEnum>(this TEnum[] enumValues) where TEnum : struct, Enum
    {
        return enumValues.Select(e => e.ToString()).ToArray();
    }

    public static TEnum[] ToTypedEnumArray<TEnum>(this int[] intValues) where TEnum : struct, Enum
    {
        return intValues.Select(s => (TEnum)Enum.ToObject(typeof(TEnum), s)).ToArray();
    }

    public static TEnum[] ToTypedEnumArray<TEnum>(this IReadOnlyCollection<int> intValues) where TEnum : struct, Enum
    {
        return intValues.Select(s => (TEnum)Enum.ToObject(typeof(TEnum), s)).ToArray();
    }

    public static TEnum ToTypedEnum<TEnum>(this int intValue) where TEnum : struct, Enum
    {
        return (TEnum)Enum.ToObject(typeof(TEnum), intValue);
    }

    public static Result<TEnum> ToTypedEnum<TEnum>(this string value) where TEnum : struct, Enum
    {
        if (!Enum.IsDefined(typeof(TEnum), value))
        {
            return Result.Failure<TEnum>(HttpStatusCode.BadRequest,
                new Error($"{typeof(TEnum).Name}.InvalidEnumValue",
                    $"Giá trị '{value}' không tồn tại cho enum '{typeof(TEnum).Name}'"));
        }

        return Result.Success((TEnum)Enum.Parse(typeof(TEnum), value));
    }

    public static Result<List<TEnum>> ToTypedEnumList<TEnum>(this IEnumerable<string> values) where TEnum : struct, Enum
    {
        var enumList = new List<TEnum>();
        foreach (var value in values)
        {
            if (!Enum.IsDefined(typeof(TEnum), value))
            {
                return Result.Failure<List<TEnum>>(HttpStatusCode.BadRequest,
                    new Error($"{typeof(TEnum).Name}.InvalidEnumValue",
                        $"Giá trị '{value}' không tồn tại cho enum '{typeof(TEnum).Name}'"));
            }

            enumList.Add((TEnum)Enum.Parse(typeof(TEnum), value));
        }

        return Result.Success(enumList);
    }
}