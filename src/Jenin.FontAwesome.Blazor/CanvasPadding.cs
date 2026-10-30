//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// The Font Awesome Icon Canvas padding utility. Applies breathing room around an icon inside
/// its canvas so that neighboring content does not collide with the fixed icon width.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/icon-canvas">The Icon Canvas</a>
/// </para>
/// </summary>
public enum CanvasPadding {
    /// <summary>
    /// No canvas padding class is applied.
    /// </summary>
    [Value("")]
    NotSet = 0,

    /// <summary>
    /// Adds square (uniform) padding inside the Icon Canvas (<c>fa-canvas-square</c>).
    /// </summary>
    [Value("fa-canvas-square")]
    Square = 1,

    /// <summary>
    /// Adds roomier padding inside the Icon Canvas (<c>fa-canvas-roomy</c>).
    /// </summary>
    [Value("fa-canvas-roomy")]
    Roomy = 2,
}
