//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Explorer.Components;

/// <summary>
/// Renders honest rendering caveats for the current preview selection, so a Pro-only style or
/// typeface never produces a silently blank or misleading preview under the Free assets the sample
/// ships. The snippet is still generated for the selected values.
/// </summary>
/// <remarks>
/// This mirrors the README feature matrix: Duotone/Sharp/Light/Thin require Font Awesome Pro, and
/// transforms/masks/layers require the SVG rendering mode. The sample host runs in SVG mode, so only
/// the Pro caveat is expected to appear in practice; the webfont caveat is emitted when the host
/// signals webfont mode through <see cref="WebFontMode"/>.
/// </remarks>
public partial class UnsupportedNotice {
    private static readonly Typeface[] _proTypefaces = [Typeface.Sharp, Typeface.Duotone, Typeface.SharpDuotone];

    /// <summary>
    /// Gets or sets the preview style.
    /// </summary>
    [Parameter]
    public IconStyle Style { get; set; }

    /// <summary>
    /// Gets or sets the preview typeface.
    /// </summary>
    [Parameter]
    public Typeface Typeface { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the host renders icons with webfonts rather than the
    /// Font Awesome SVG engine, in which case transforms, masks, and layers are inert.
    /// </summary>
    [Parameter]
    public bool WebFontMode { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a transform, mask, or duotone layer is currently
    /// selected (only relevant in webfont mode).
    /// </summary>
    [Parameter]
    public bool UsesSvgOnlyFeature { get; set; }

    private IReadOnlyList<(string Badge, string Text)> Messages {
        get {
            var messages = new List<(string, string)>();

            if(IsProStyle(Style)) {
                messages.Add(("Pro", $"The {Style} style requires Font Awesome Pro. With the Free assets this host ships, the icon falls back to the equivalent classic solid style."));
            }

            if(_proTypefaces.Contains(Typeface)) {
                messages.Add(("Pro", $"The {Typeface} typeface requires Font Awesome Pro. With the Free assets this host ships, the icon falls back to the classic (non-sharp) style."));
            }

            if(WebFontMode && UsesSvgOnlyFeature) {
                messages.Add(("SVG", "Transforms, masks, and duotone layers only render with the Font Awesome SVG engine. This host uses webfonts, so these effects are inert here."));
            }

            return messages;
        }
    }

    private static bool IsProStyle(IconStyle style) =>
        style is IconStyle.Light or IconStyle.Thin or IconStyle.Duotone;
}
