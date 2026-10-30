//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// Represents a different look of the same icons.
/// <para>
/// <a href="https://docs.fontawesome.com/apis/javascript/import-icons/">Referencing Icons [Style]</a>
/// </para>
/// </summary>
public enum IconStyle {
    /// <summary>
    /// Icon will be filled with single-color.
    /// </summary>
    [Value("fa-solid")]
    Solid = 0,

    /// <summary>
    /// Icon will be outlined with single-color.
    /// </summary>
    [Value("fa-regular")]
    Regular = 1,

    /// <summary>
    /// Icon will be slightly lighter.
    /// </summary>
    [Value("fa-light")]
    Light = 2,

    /// <summary>
    /// Icon will be thinner.
    /// </summary>
    [Value("fa-thin")]
    Thin = 3,

    /// <summary>
    /// Icon will be shown in two-color tones. In Font Awesome 7 this token is the Duotone family
    /// prefix, so the FA 7 rendering contract pairs it with a style class: emitting the canonical
    /// duotone-solid markup <c>fa-duotone fa-solid</c>. For other duotone styles set
    /// <see cref="Typeface.Duotone"/> or <see cref="Typeface.SharpDuotone"/> instead and keep this
    /// property on a plain style value such as <see cref="Solid"/> or <see cref="Light"/>. Requires
    /// Font Awesome Pro; with Free assets the icon renders the equivalent classic solid style.
    /// <para>
    /// <a href="https://docs.fontawesome.com/web/style/duotone">Duotone Icons</a>
    /// </para>
    /// </summary>
    [Value("fa-duotone fa-solid")]
    Duotone = 4,

    /// <summary>
    /// It will use the symbols of well-known brands
    /// </summary>
    [Value("fa-brands")]
    Brands = 5,
}
