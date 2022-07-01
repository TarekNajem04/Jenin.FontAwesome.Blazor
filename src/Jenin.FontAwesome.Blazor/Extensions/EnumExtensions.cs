using Jenin.FontAwesome.Blazor;
using Jenin.FontAwesome.Blazor.Extensions;

namespace Jenin.FontAwesome.Blazor.Extensions;

public static class EnumExtensions {
    public static string GetStringValue(this Enum value, string defaultValue = default) => value.GetValue(defaultValue);

    public static TValue GetValue<TValue>(this Enum value, TValue defaultValue = default) => value.GetValue() is TValue enumValue ? enumValue : defaultValue;
    public static object GetValue(this Enum value) => value.GetCustomAttribute<ValueAttribute>()?.Value;
    public static string GetDescription(this Enum value) => value.GetCustomAttribute<DescriptionAttribute>()?.Description ?? value.ToString();
    public static T GetCustomAttribute<T>(this Enum value) where T : Attribute => value.GetType().GetMember(value.ToString())[0].GetCustomAttribute<T>();

    public static bool HasCustomAttribute<T>(this Enum value) where T : Attribute => value.GetCustomAttribute<T>() is not null;
    public static bool NotHasCustomAttribute<T>(this Enum value) where T : Attribute => !value.HasCustomAttribute<T>();

    public static TEnum ToEnum<TEnum>(this string value, TEnum defaultValue = default) where TEnum : struct
        => string.IsNullOrEmpty(value)
                ? defaultValue
                : Enum.TryParse<TEnum>(value, out var enumValue) ? enumValue : defaultValue;

    public static IEnumerable<TEnum> GetAllFlags<TEnum>(this TEnum flags) where TEnum : Enum {
        ulong flag = 1;

        foreach (var value in Enum.GetValues(flags.GetType()).Cast<TEnum>()) {
            var bits = Convert.ToUInt64(value);

            while (flag < bits) {
                flag <<= 1;
            }

            if (flag == bits && flags.HasFlag(value)) {
                yield return value;
            }
        }
    }
}