//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.MudBlazor;

/// <summary>
/// Maps typed Font Awesome names to the strings MudBlazor's <c>MudIcon</c> slot consumes
/// (Font Awesome CSS classes or null-safe fallbacks).
/// </summary>
/// <remarks>
/// MudBlazor renders <c>Icon</c> values either as inline SVG (angle-bracket payloads from
/// <c>Icons.Material.*</c>) or as Font Awesome CSS classes. This mapper covers the second
/// path; pass the result straight to <c>MudIcon.Icon</c>, <c>MudIconButton.Icon</c>,
/// <c>MudNavLink.Icon</c>, and friends. Custom kit glyphs flow through unchanged.
/// </remarks>
public static class MudIconMapper {
    /// <summary>
    /// Converts an <c>IconName</c> value to the Font Awesome CSS class string MudBlazor
    /// expects (for example <c>fa-house</c> + <see cref="IconStyle.Solid"/> becomes
    /// <c>fa-solid fa-house</c>). Unknown and custom names pass through untouched.
    /// </summary>
    /// <param name="iconName">The <c>IconName</c> value, with or without the <c>fa-</c> prefix.</param>
    /// <param name="style">The Font Awesome style; selects the class prefix.</param>
    /// <returns>The CSS class string for <c>MudIcon.Icon</c>.</returns>
    public static string ToFontClass(string? iconName, IconStyle style = IconStyle.Solid) {
        if(string.IsNullOrWhiteSpace(iconName)) {
            return string.Empty;
        }

        var name = iconName.Trim();

        if(!name.StartsWith("fa-", StringComparison.OrdinalIgnoreCase)) {
            name = "fa-" + name;
        }

        return ToStylePrefix(style) + " " + name;
    }

    /// <summary>
    /// Maps an <see cref="IconSize"/> to the closest MudBlazor <c>Size</c>.
    /// Only the named steps overlap; every other value (including multipliers) returns
    /// <c>null</c> so callers keep MudBlazor's default and size via CSS instead.
    /// </summary>
    /// <param name="size">The Font Awesome size.</param>
    /// <returns>The closest MudBlazor size, or <c>null</c> when there is none.</returns>
    public static global::MudBlazor.Size? ToMudSize(IconSize? size) => size switch {
        IconSize.Small => global::MudBlazor.Size.Small,
        IconSize.Large => global::MudBlazor.Size.Large,
        _ => null,
    };

    internal static string ToStylePrefix(IconStyle style) => style switch {
        IconStyle.Regular => "fa-regular",
        IconStyle.Light => "fa-light",
        IconStyle.Thin => "fa-thin",
        IconStyle.Duotone => "fa-duotone",
        IconStyle.Brands => "fa-brands",
        _ => "fa-solid",
    };
}
