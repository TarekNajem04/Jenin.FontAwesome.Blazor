//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Extensions;

internal static class DictionaryExtensions {
    public static Dictionary<TKey, TValue> Combine<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value, Func<TValue, TValue, TValue>? concat = null) where TKey : notnull {
        if(key is null || value is null) {
            return dictionary;
        }

        if(dictionary.TryGetValue(key, out var existValue)) {
            dictionary[key] = concat is null ? (TValue)Concat(value, existValue) : concat(value, existValue);
        } else {
            dictionary.Add(key, value);
        }

        return dictionary;
    }

    public static Dictionary<TKey, string> Combine<TKey>(this Dictionary<TKey, string> dictionary, TKey key, string value) where TKey : notnull
        => dictionary.Combine(key, value, (value, existValue) => string.IsNullOrEmpty(existValue) ? value : string.Concat(value, existValue));

    public static object Concat(object? value, object? existValue) {
        var existValueString = Convert.ToString(existValue, CultureInfo.InvariantCulture);

        if(string.IsNullOrEmpty(existValueString)) {
            return value ?? string.Empty;
        }

        var valueString = Convert.ToString(value, CultureInfo.InvariantCulture);

        return string.Concat(valueString, existValueString);
    }

    public static Dictionary<TKey, TValue> AddIfNotNull<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value) where TKey : notnull {
        if(key is null || value is null) {
            return dictionary;
        }

        dictionary.Add(key, value);

        return dictionary;
    }

    public static Dictionary<TKey, string> AddIfNotNull<TKey>(this Dictionary<TKey, string> dictionary, TKey key, string? value) where TKey : notnull {
        if(key is null || string.IsNullOrEmpty(value)) {
            return dictionary;
        }

        dictionary.Add(key, value);

        return dictionary;
    }

    public static Dictionary<TKey, string> AddIfNotNull<TKey>(this Dictionary<TKey, string> dictionary, TKey key, float? value) where TKey : notnull {
        if(key is null || !value.HasValue) {
            return dictionary;
        }

        dictionary.Add(key, value.Value.ToString("F", CultureInfo.InvariantCulture));

        return dictionary;
    }

    public static Dictionary<TKey, string> AddIfNotNull<TKey>(this Dictionary<TKey, string> dictionary, TKey key, int? value) where TKey : notnull {
        if(key is null || !value.HasValue) {
            return dictionary;
        }

        dictionary.Add(key, value.Value.ToString(CultureInfo.InvariantCulture));

        return dictionary;
    }

    public static Dictionary<TKey, TValue> AddIf<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value, Func<TValue, bool> predicate) where TKey : notnull {
        if(key is null || !predicate(value)) {
            return dictionary;
        }

        dictionary.Add(key, value);

        return dictionary;
    }

    public static Dictionary<TKey, string> AddIf<TKey>(this Dictionary<TKey, string> dictionary, TKey key, string value, Func<string, bool> predicate) where TKey : notnull {
        if(key is null || !predicate(value) || string.IsNullOrEmpty(value)) {
            return dictionary;
        }

        dictionary.Add(key, value);

        return dictionary;
    }

    public static Dictionary<TKey, string> AddIf<TKey, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] TEnum>(this Dictionary<TKey, string> dictionary, TKey key, TEnum value, Func<TEnum, bool> predicate) where TKey : notnull where TEnum : struct, Enum {
        if(key is null || !predicate(value)) {
            return dictionary;
        }

        dictionary.Add(key, value.GetStringValue() ?? string.Empty);

        return dictionary;
    }

    public static Dictionary<TKey, TValue> AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value) where TKey : notnull {
        if(key is null || value is null) {
            return dictionary;
        }

        dictionary[key] = value;

        return dictionary;
    }

    public static Dictionary<TKey, string> AddOrUpdate<TKey>(this Dictionary<TKey, string> dictionary, TKey key, string value) where TKey : notnull {
        if(key is null || string.IsNullOrEmpty(value)) {
            return dictionary;
        }

        dictionary[key] = value;

        return dictionary;
    }

    public static Dictionary<TKey, TValue> AddOrUpdateIf<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value, Func<object, bool> predicate) where TKey : notnull {
        if(key is null || value is null || !predicate(value)) {
            return dictionary;
        }

        dictionary[key] = value;

        return dictionary;
    }

    public static Dictionary<TKey, string> AddOrUpdateIf<TKey, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields)] TEnum>(this Dictionary<TKey, string> dictionary, TKey key, TEnum value, Func<TEnum, bool> predicate) where TKey : notnull where TEnum : struct, Enum {
        if(key is null || !predicate(value)) {
            return dictionary;
        }

        dictionary[key] = value.GetStringValue() ?? string.Empty;

        return dictionary;
    }

    public static Dictionary<TKey, string> AddOrUpdateIfNotNull<TKey>(this Dictionary<TKey, string> dictionary, TKey key, string? value) where TKey : notnull {
        if(key is null || string.IsNullOrEmpty(value)) {
            return dictionary;
        }

        dictionary[key] = value;

        return dictionary;
    }

    public static Dictionary<TKey, string> AddOrUpdateIfNotNull<TKey>(this Dictionary<TKey, string> dictionary, TKey key, float? value) where TKey : notnull {
        if(key is null || !value.HasValue) {
            return dictionary;
        }

        dictionary[key] = value.Value.ToString("F", CultureInfo.InvariantCulture);

        return dictionary;
    }

    public static Dictionary<TKey, string> AddOrUpdateIfNotNull<TKey>(this Dictionary<TKey, string> dictionary, TKey key, int? value) where TKey : notnull {
        if(key is null || !value.HasValue) {
            return dictionary;
        }

        dictionary[key] = value.Value.ToString(CultureInfo.InvariantCulture);

        return dictionary;
    }

    /// <summary>
    /// Sets the value when <paramref name="value"/> is non-empty, otherwise removes the key. Use this
    /// for keys that are recomputed on every render from a persistent dictionary (for example the
    /// emitted <c>class</c> and <c>style</c> attributes): the additive helpers only write when the
    /// value is non-empty, so a value that later becomes empty would linger and be re-emitted.
    /// </summary>
    /// <typeparam name="TKey">The dictionary key type.</typeparam>
    /// <typeparam name="TValue">The dictionary value type.</typeparam>
    /// <param name="dictionary">The dictionary to update.</param>
    /// <param name="key">The key to set or remove.</param>
    /// <param name="value">The value; when null or empty the key is removed.</param>
    /// <returns>The same dictionary instance.</returns>
    public static Dictionary<TKey, TValue> SetOrRemoveIfEmpty<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, string? value) where TKey : notnull {
        if(key is null) {
            return dictionary;
        }

        if(string.IsNullOrEmpty(value)) {
            _ = dictionary.Remove(key);
        } else {
            dictionary[key] = (TValue)(object)value;
        }

        return dictionary;
    }

    public static Dictionary<TKey, TValue> UpdateOrRemoveIf<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue? value, Func<TValue, bool>? predicate = null) where TKey : notnull {
        if(key is null) {
            return dictionary;
        }

        if(value is null) {
            _ = dictionary.Remove(key);

            return dictionary;
        }

        if(predicate?.Invoke(value) == true) {
            _ = dictionary.Remove(key);
        } else {
            dictionary[key] = value;
        }

        return dictionary;
    }

    public static Dictionary<TKey, string> UpdateOrRemoveIfNull<TKey>(this Dictionary<TKey, string> dictionary, TKey key, string value) where TKey : notnull
        => dictionary.UpdateOrRemoveIf(key, value, _ => string.IsNullOrEmpty(value));

    public static string JoinAsStyles<TKey>(this Dictionary<TKey, string> dictionary) where TKey : notnull
        => dictionary.Count > 0
             ? dictionary.Aggregate
                         (
                             new StringBuilder(),
                             (sb, kvp) => string.IsNullOrEmpty(kvp.Value)
                                             ? sb
                                             : sb.Append(kvp.Key)
                                                 .Append(':')
                                                 .Append(kvp.Value)
                                                 .Append(';')
                         )
                         .ToString()
             : string.Empty;
}
