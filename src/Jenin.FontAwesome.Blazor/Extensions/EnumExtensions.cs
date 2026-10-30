//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Extensions;

internal static class EnumExtensions {
    public static string? GetStringValue<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] TEnum>(this TEnum value, string? defaultValue = null) where TEnum : struct, Enum
        => value.GetField()?.GetCustomAttribute<ValueAttribute>()?.Value as string ?? defaultValue;

    public static TValue? GetValue<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] TEnum, TValue>(this TEnum value, TValue? defaultValue = default) where TEnum : struct, Enum
        => value.GetField()?.GetCustomAttribute<ValueAttribute>()?.Value is TValue enumValue ? enumValue : defaultValue;

    public static string GetDescription<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] TEnum>(this TEnum value) where TEnum : struct, Enum
        => value.GetField()?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? value.ToString();

    /// <summary>
    /// Resolves the enum member's backing <see cref="FieldInfo"/>.
    /// </summary>
    /// <remarks>
    /// The enum type is supplied as a generic type parameter annotated with
    /// <see cref="DynamicallyAccessedMemberTypes.PublicFields"/> (rather than reading
    /// <see cref="object.GetType"/> on a boxed <see cref="Enum"/>). This keeps custom-attribute
    /// lookup trim- and Native AOT-safe: the analyzer can prove the field annotations are preserved,
    /// and no dynamic-code path is required.
    /// </remarks>
    public static FieldInfo? GetField<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] TEnum>(this TEnum value) where TEnum : struct, Enum
        => typeof(TEnum).GetField(value.ToString());

    public static TEnum ToEnum<TEnum>(this string value, TEnum defaultValue = default) where TEnum : struct
        => string.IsNullOrEmpty(value)
                ? defaultValue
                : Enum.TryParse<TEnum>(value, out var enumValue) ? enumValue : defaultValue;

    public static IEnumerable<TEnum> GetAllFlags<TEnum>(this TEnum flags) where TEnum : struct, Enum {
        ulong flag = 1;

        foreach(var value in Enum.GetValues<TEnum>()) {
            var bits = Convert.ToUInt64(value, CultureInfo.InvariantCulture);

            while(flag < bits) {
                flag <<= 1;
            }

            if(flag == bits && flags.HasFlag(value)) {
                yield return value;
            }
        }
    }
}
