//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// sets the line style for all four sides of an element's border.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/pull">Bordered and Pulled Classes</a>
/// </para>
/// </summary>
public enum BorderStyle {
    /// <summary>
    /// No border is displayed (border-style: none).
    /// </summary>
    [Value("none")]
    None = 0,

    /// <summary>
    /// The border is hidden (border-style: hidden), taking no space.
    /// </summary>
    [Value("hidden")]
    Hidden = 1,

    /// <summary>
    /// A dotted border (border-style: dotted).
    /// </summary>
    [Value("dotted")]
    Dotted = 2,

    /// <summary>
    /// A dashed border (border-style: dashed).
    /// </summary>
    [Value("dashed")]
    Dashed = 3,

    /// <summary>
    /// A solid border (border-style: solid).
    /// </summary>
    [Value("solid")]
    Solid = 4,

    /// <summary>
    /// A double-line border (border-style: double).
    /// </summary>
    [Value("double")]
    Double = 5,

    /// <summary>
    /// A grooved border (border-style: groove).
    /// </summary>
    [Value("groove")]
    Groove = 6,

    /// <summary>
    /// A ridged border (border-style: ridge).
    /// </summary>
    [Value("ridge")]
    Ridge = 7,

    /// <summary>
    /// An inset border (border-style: inset).
    /// </summary>
    [Value("inset")]
    Inset = 8,

    /// <summary>
    /// An outset border (border-style: outset).
    /// </summary>
    [Value("outset")]
    Outset = 9
}
