//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// Sharp is built on a 16 pixel grid but was intentionally cut out as much rounding as reasonably possible.
/// <para>
/// <a href="https://blog.fontawesome.com/introducing-font-awesome-sharp/">Introducing Font Awesome Sharp</a>
/// </para>
/// </summary>
/// <remarks>
/// Font Awesome 7 models icon markup as a family class (this enum) paired with a style class
/// (<see cref="IconStyle"/>); the classic family ships no class and is the default. For Duotone/Sharp
/// Duotone families a matching Pro asset kit is required — with Free assets the icon falls back to the
/// equivalent classic style.
/// </remarks>
public enum Typeface {
    /// <summary>
    /// The icon will be drawn with a rounded edge.
    /// </summary>
    [Value("")]
    Classic = 0,

    /// <summary>
    /// The icon will be drawn with a sharp edge.
    /// </summary>
    [Value("fa-sharp")]
    Sharp = 1,

    /// <summary>
    /// The icon will be drawn in two-color tones. Pair with the desired icon style
    /// (<see cref="IconStyle.Solid"/>, Regular, Light, Thin). Requires Font Awesome Pro.
    /// <para>
    /// <a href="https://docs.fontawesome.com/web/style/duotone">Duotone Icons</a>
    /// </para>
    /// </summary>
    [Value("fa-duotone")]
    Duotone = 2,

    /// <summary>
    /// The icon will be drawn in the Sharp Duotone family. Pair with the desired icon style. Requires
    /// Font Awesome Pro.
    /// <para>
    /// <a href="https://docs.fontawesome.com/web/style/duotone">Duotone Icons</a>
    /// </para>
    /// </summary>
    [Value("fa-sharp-duotone")]
    SharpDuotone = 3,
}
