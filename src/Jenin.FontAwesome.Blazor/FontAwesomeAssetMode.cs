//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// The Font Awesome delivery mode the one-line setup wires assets for.
/// </summary>
public enum FontAwesomeAssetMode {
    /// <summary>
    /// SVG enhancement with nesting: the Font Awesome engine replaces <c>&lt;i&gt;</c> anchors
    /// with inline SVG (<c>autoReplaceSvg: "nest"</c>). Required for layers, masks, power
    /// transforms, and duotone; safe with Blazor rendering.
    /// </summary>
    SvgNest = 0,

    /// <summary>
    /// Static webfont mode: glyphs render through the Free typeface
    /// (<c>autoReplaceSvg: false</c>). Use only when SVG enhancement is unavailable; engine
    /// features such as layers are inert in this mode.
    /// </summary>
    Webfont = 1,
}
