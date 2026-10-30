//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Analyzers;

/// <summary>
/// Parses and converts raw Font Awesome class strings (JFA1002). A class string is a style
/// token plus an icon token (<c>fa-solid fa-house</c>), optionally followed by size, animation,
/// or modifier tokens that do not belong in a name position.
/// </summary>
internal static class ClassString {
    private static readonly char[] _separators = ['-', '_'];

    public static bool TryParse(string value, out string style, out string name, out int extraTokens) {
        style = string.Empty;
        name = string.Empty;
        extraTokens = 0;

        var tokens = value.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
        if(tokens.Length < 2
            || !IsStyleToken(tokens[0], out var styleName)
            || !IsNameToken(tokens[1])) {
            return false;
        }

        style = styleName;
        name = tokens[1];
        extraTokens = tokens.Length - 2;
        return true;
    }

    public static bool IsStyleToken(string token, out string style) {
        style = token switch {
            "fa-solid" => "Solid",
            "fa-regular" => "Regular",
            "fa-brands" => "Brands",
            "fa-light" => "Light",
            "fa-thin" => "Thin",
            "fa-duotone" => "Duotone",
            _ => string.Empty,
        };
        return style.Length > 0;
    }

    public static bool IsNameToken(string token) =>
        token.StartsWith("fa-", StringComparison.Ordinal) && token.Length > 3;

    /// <summary>
    /// Converts a bare icon name to its <c>IconNames</c> member name (<c>fa-house</c> to
    /// <c>House</c>, <c>fa-500px</c> to <c>_500px</c>), mirroring the catalogue generator.
    /// </summary>
    public static string ToMemberName(string bareName) {
        var builder = new StringBuilder(bareName.Length);
        foreach(var part in bareName.Split(_separators, StringSplitOptions.RemoveEmptyEntries)) {
            builder.Append(char.ToUpperInvariant(part[0]));
            if(part.Length > 1) {
                builder.Append(part.Substring(1));
            }
        }

        var member = builder.ToString();
        return member.Length > 0 && char.IsDigit(member[0]) ? "_" + member : member;
    }
}
