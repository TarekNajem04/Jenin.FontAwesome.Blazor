namespace Jenin.FontAwesome.Blazor.Extensions;

public static class DictionaryExtensions {
    public static Dictionary<Tkey, TValue> Combine<Tkey, TValue>(this Dictionary<Tkey, TValue> dictionary, Tkey key, TValue value, Func<TValue, TValue, TValue> concat = default) {
        if (dictionary is null || key is null || value is null) {
            return dictionary;
        }

        if (dictionary.ContainsKey(key)) {
            var concatenated = concat is null ? (TValue)Concat(value, dictionary[key]) : concat(value, dictionary[key]);

            dictionary[key] = concatenated;
        } else {
            dictionary.Add(key, value);
        }

        return dictionary;
    }

    public static Dictionary<Tkey, string> Combine<Tkey>(this Dictionary<Tkey, string> dictionary, Tkey key, string value)
        => dictionary.Combine(key, value, (value, existValue) => string.IsNullOrEmpty(existValue) ? value : string.Concat(value, existValue));

    public static object Concat(object value, object existValue) {
        var existValueString = Convert.ToString(existValue, CultureInfo.InvariantCulture);

        if (string.IsNullOrEmpty(existValueString)) {
            return value;
        }

        var valueString = Convert.ToString(value, CultureInfo.InvariantCulture);

        return string.Concat(valueString, existValueString);
    }

    public static Dictionary<Tkey, TValue> AddIfNotNull<Tkey, TValue>(this Dictionary<Tkey, TValue> dictionary, Tkey key, TValue value) {
        if (dictionary is null || key is null || value is null) {
            return dictionary;
        }

        dictionary.Add(key, value);

        return dictionary;
    }

    public static Dictionary<Tkey, string> AddIfNotNull<Tkey>(this Dictionary<Tkey, string> dictionary, Tkey key, string value) {
        if (dictionary is null || key is null || string.IsNullOrEmpty(value)) {
            return dictionary;
        }

        dictionary.Add(key, value);

        return dictionary;
    }

    public static Dictionary<Tkey, string> AddIfNotNull<Tkey>(this Dictionary<Tkey, string> dictionary, Tkey key, float? value) {
        if (dictionary is null || key is null || !value.HasValue) {
            return dictionary;
        }

        dictionary.Add(key, value.Value.ToString("F", CultureInfo.InvariantCulture));

        return dictionary;
    }

    public static Dictionary<Tkey, string> AddIfNotNull<Tkey>(this Dictionary<Tkey, string> dictionary, Tkey key, int? value) {
        if (dictionary is null || key is null || !value.HasValue) {
            return dictionary;
        }

        dictionary.Add(key, value.Value.ToString());

        return dictionary;
    }

    public static Dictionary<Tkey, TValue> AddIf<Tkey, TValue>(this Dictionary<Tkey, TValue> dictionary, Tkey key, TValue value, Func<TValue, bool> predicate) {
        if (dictionary is null || key is null || predicate?.Invoke(value) != true) {
            return dictionary;
        }

        dictionary.Add(key, value);

        return dictionary;
    }

    public static Dictionary<Tkey, string> AddIf<Tkey>(this Dictionary<Tkey, string> dictionary, Tkey key, string value, Func<string, bool> predicate) {
        if (dictionary is null || key is null || predicate?.Invoke(value) != true || string.IsNullOrEmpty(value)) {
            return dictionary;
        }

        dictionary.Add(key, value);

        return dictionary;
    }

    public static Dictionary<Tkey, string> AddIf<Tkey>(this Dictionary<Tkey, string> dictionary, Tkey key, Enum value, Func<Enum, bool> predicate) {
        if (dictionary is null || key is null || predicate?.Invoke(value) != true) {
            return dictionary;
        }

        dictionary.Add(key, value.GetStringValue());

        return dictionary;
    }

    public static Dictionary<Tkey, TValue> AddOrUpdate<Tkey, TValue>(this Dictionary<Tkey, TValue> dictionary, Tkey key, TValue value) {
        if (dictionary is null || key is null || value is null) {
            return dictionary;
        }

        dictionary[key] = value;

        return dictionary;
    }

    public static Dictionary<Tkey, string> AddOrUpdate<Tkey>(this Dictionary<Tkey, string> dictionary, Tkey key, string value) {
        if (dictionary is null || key is null || string.IsNullOrEmpty(value)) {
            return dictionary;
        }

        dictionary[key] = value;

        return dictionary;
    }

    public static Dictionary<Tkey, TValue> AddOrUpdateIf<Tkey, TValue>(this Dictionary<Tkey, TValue> dictionary, Tkey key, TValue value, Func<object, bool> predicate) {
        if (dictionary is null || key is null || value is null || predicate?.Invoke(value) != true) {
            return dictionary;
        }

        dictionary[key] = value;

        return dictionary;
    }
    public static Dictionary<Tkey, string> AddOrUpdateIfNotNull<Tkey>(this Dictionary<Tkey, string> dictionary, Tkey key, string value)
        => dictionary.AddOrUpdateIf(key, value, _ => !string.IsNullOrEmpty(value));

    public static Dictionary<Tkey, string> AddOrUpdateIf<Tkey>(this Dictionary<Tkey, string> dictionary, Tkey key, Enum value, Func<Enum, bool> predicate) {
        if (dictionary is null || key is null || predicate?.Invoke(value) != true) {
            return dictionary;
        }

        dictionary[key] = value.GetStringValue();

        return dictionary;
    }

    public static Dictionary<Tkey, string> AddOrUpdateIfNotNull<Tkey>(this Dictionary<Tkey, string> dictionary, Tkey key, float? value) {
        if (dictionary is null || key is null || !value.HasValue) {
            return dictionary;
        }

        dictionary[key] = value.Value.ToString("F", CultureInfo.InvariantCulture);

        return dictionary;
    }

    public static Dictionary<Tkey, string> AddOrUpdateIfNotNull<Tkey>(this Dictionary<Tkey, string> dictionary, Tkey key, int? value) {
        if (dictionary is null || key is null || !value.HasValue) {
            return dictionary;
        }

        dictionary[key] = value.Value.ToString();

        return dictionary;
    }

    public static Dictionary<Tkey, TValue> UpdateOrRemoveIf<Tkey, TValue>(this Dictionary<Tkey, TValue> dictionary, Tkey key, TValue value, Func<TValue, bool> predicate = default) {
        if (dictionary is null || key is null) {
            return dictionary;
        }

        if (predicate?.Invoke(value) == true || value is null) {
            _ = dictionary.Remove(key);
        } else {
            dictionary[key] = value;
        }

        return dictionary;
    }

    public static Dictionary<Tkey, string> UpdateOrRemoveIfNull<Tkey>(this Dictionary<Tkey, string> dictionary, Tkey key, string value)
        => dictionary.UpdateOrRemoveIf(key, value, _ => string.IsNullOrEmpty(value));

    public static string JoinAsStyles<Tkey>(this Dictionary<Tkey, string> dictionary)
        => dictionary?.Count > 0
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