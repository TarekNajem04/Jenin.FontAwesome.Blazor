//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Blazorise;

/// <summary>
/// Maps typed Font Awesome values onto the Blazorise <c>Icon</c> component contract
/// (<c>Name</c>, <c>IconStyle</c>, <c>IconSize</c>).
/// </summary>
/// <remarks>
/// Blazorise accepts plain FA class strings in <c>Icon.Name</c> when the Font Awesome icon
/// provider is registered (<c>AddFontAwesomeIcons()</c>). Unknown and custom kit names pass
/// through untouched, exactly as Blazorise's own custom-name support does.
/// </remarks>
public static class BlazoriseIconMapper {
    /// <summary>
    /// Converts a <c>IconName</c> value to the string Blazorise's <c>Icon.Name</c> accepts
    /// (custom names are returned unchanged).
    /// </summary>
    /// <param name="iconName">The <c>IconName</c> value, with or without the <c>fa-</c> prefix.</param>
    /// <returns>The name string for <c>Icon.Name</c>.</returns>
    public static string ToIconName(string? iconName) {
        if(string.IsNullOrWhiteSpace(iconName)) {
            return string.Empty;
        }

        var name = iconName.Trim();

        return name.StartsWith("fa-", StringComparison.OrdinalIgnoreCase) ? name : "fa-" + name;
    }

    /// <summary>
    /// Maps a Font Awesome icon style to the Blazorise <c>IconStyle</c>. <c>null</c>,
    /// <c>Thin</c>, and <c>Brands</c> have no Blazorise counterpart and return <c>null</c>
    /// (provider default applies — document the gap).
    /// </summary>
    /// <param name="style">The Font Awesome style, or <c>null</c> for the provider default.</param>
    /// <returns>The Blazorise style, or <c>null</c> when unmapped.</returns>
    public static global::Blazorise.IconStyle? ToBlazoriseStyle(global::Jenin.FontAwesome.Blazor.IconStyle? style) => style switch {
        global::Jenin.FontAwesome.Blazor.IconStyle.Solid => global::Blazorise.IconStyle.Solid,
        global::Jenin.FontAwesome.Blazor.IconStyle.Regular => global::Blazorise.IconStyle.Regular,
        global::Jenin.FontAwesome.Blazor.IconStyle.Light => global::Blazorise.IconStyle.Light,
        global::Jenin.FontAwesome.Blazor.IconStyle.Duotone => global::Blazorise.IconStyle.DuoTone,
        _ => null,
    };

    /// <summary>
    /// Maps a Font Awesome icon size to the Blazorise <c>IconSize</c>. Only the shared steps
    /// map (<c>ExtraSmall</c>, <c>Small</c>, <c>Large</c>, <c>X02</c>–<c>X10</c>); everything
    /// else returns <c>null</c> so the provider default applies and callers size via CSS.
    /// </summary>
    /// <param name="size">The Font Awesome size.</param>
    /// <returns>The Blazorise size, or <c>null</c> when unmapped.</returns>
    public static global::Blazorise.IconSize? ToBlazoriseSize(global::Jenin.FontAwesome.Blazor.IconSize? size) => size switch {
        global::Jenin.FontAwesome.Blazor.IconSize.ExtraSmall => global::Blazorise.IconSize.ExtraSmall,
        global::Jenin.FontAwesome.Blazor.IconSize.Small => global::Blazorise.IconSize.Small,
        global::Jenin.FontAwesome.Blazor.IconSize.Large => global::Blazorise.IconSize.Large,
        global::Jenin.FontAwesome.Blazor.IconSize.X02 => global::Blazorise.IconSize.x2,
        global::Jenin.FontAwesome.Blazor.IconSize.X03 => global::Blazorise.IconSize.x3,
        global::Jenin.FontAwesome.Blazor.IconSize.X04 => global::Blazorise.IconSize.x4,
        global::Jenin.FontAwesome.Blazor.IconSize.X05 => global::Blazorise.IconSize.x5,
        global::Jenin.FontAwesome.Blazor.IconSize.X06 => global::Blazorise.IconSize.x6,
        global::Jenin.FontAwesome.Blazor.IconSize.X07 => global::Blazorise.IconSize.x7,
        global::Jenin.FontAwesome.Blazor.IconSize.X08 => global::Blazorise.IconSize.x8,
        global::Jenin.FontAwesome.Blazor.IconSize.X09 => global::Blazorise.IconSize.x9,
        global::Jenin.FontAwesome.Blazor.IconSize.X10 => global::Blazorise.IconSize.x10,
        _ => null,
    };
}
