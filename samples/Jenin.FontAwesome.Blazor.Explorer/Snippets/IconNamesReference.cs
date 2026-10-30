//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Explorer.Snippets;

/// <summary>
/// Resolves an icon to its strongly-typed <see cref="IconNames"/> constant, for example
/// <c>IconNames.Solid.House</c>, using the same identifier algorithm as the catalogue generator.
/// </summary>
/// <remarks>
/// The generated <see cref="IconNames"/> type exposes three nested groups — <c>Solid</c>,
/// <c>Regular</c>, and <c>Brands</c> — each holding a constant per icon whose name is the canonical
/// name with every <c>-</c>/<c>_</c> segment capitalised (a leading digit is prefixed with <c>_</c>).
/// The reference is emitted with the group matching the icon's selected style, falling back to the
/// first Free style in which the icon is available.
/// </remarks>
internal static class IconNamesReference {
    /// <summary>
    /// Builds the <see cref="IconNames"/> reference for <paramref name="cssClass"/> (for example
    /// <c>fa-house</c>) in <paramref name="style"/>, or <c>null</c> when no reference applies.
    /// </summary>
    /// <param name="cssClass">The icon's style-independent CSS class, including the <c>fa-</c> prefix.</param>
    /// <param name="style">The currently selected icon style.</param>
    /// <param name="styles">The Free styles in which the icon is available, in canonical order.</param>
    /// <returns>A reference such as <c>IconNames.Solid.House</c>, or <c>null</c>.</returns>
    public static string? Build(string cssClass, IconStyle style, IReadOnlyList<IconStyle> styles) {
        if(styles.Count == 0) {
            styles = [IconStyle.Solid];
        }

        if(!TryGetGroup(styles.Contains(style) ? style : styles[0], out var group)) {
            return null;
        }

        var identifier = ToIdentifier(cssClass);
        return identifier is null ? null : $"IconNames.{group}.{identifier}";
    }

    private static bool TryGetGroup(IconStyle style, out string group) {
        switch(style) {
            case IconStyle.Solid:
                group = "Solid";
                return true;
            case IconStyle.Regular:
                group = "Regular";
                return true;
            case IconStyle.Brands:
                group = "Brands";
                return true;
            default:
                group = string.Empty;
                return false;
        }
    }

    private static string? ToIdentifier(string cssClass) {
        var name = cssClass.StartsWith("fa-", StringComparison.Ordinal) ? cssClass[3..] : cssClass;
        if(name.Length == 0) {
            return null;
        }

        var builder = new System.Text.StringBuilder(name.Length);
        foreach(var part in name.Split(['-', '_'], StringSplitOptions.RemoveEmptyEntries)) {
            _ = builder.Append(char.ToUpperInvariant(part[0]));
            if(part.Length > 1) {
                _ = builder.Append(part.AsSpan(1));
            }
        }

        if(builder.Length == 0) {
            return null;
        }

        if(char.IsDigit(builder[0])) {
            _ = builder.Insert(0, '_');
        }

        return builder.ToString();
    }
}
