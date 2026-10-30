//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// Rotate, Flip, or Mirror icons.
/// https://fontawesome.com/docs/web/style/rotate
/// </summary>
public enum IconRotate {
    /// <summary>
    /// The icon rotate will not be applied.
    /// </summary>
    [Value("")]
    NotSet = 0,

    /// <summary>
    /// Rotates an icon 90°.
    /// </summary>
    [Value("fa-rotate-90")]
    Deg090 = 1,

    /// <summary>
    /// Rotates an icon 180°.
    /// </summary>
    [Value("fa-rotate-180")]
    Deg180 = 2,

    /// <summary>
    /// Rotates an icon 270°.
    /// </summary>
    [Value("fa-rotate-270")]
    Deg270 = 3,

    /// <summary>
    /// Mirrors an icon horizontally.
    /// </summary>
    [Value("fa-flip-horizontal")]
    FlipHorizontal = 4,

    /// <summary>
    /// Mirrors an icon vertically
    /// </summary>
    [Value("fa-flip-vertical")]
    FlipVertical = 5,

    /// <summary>
    /// Mirrors an icon both vertically and horizontally.
    /// </summary>
    [Value("fa-flip-both")]
    FlipBoth = 6,

    /// <summary>
    /// Rotates an icon by a specific degree,
    /// setting an accompanying valid inline value for <see cref="Icon.RotateAngle"/> is required.
    /// </summary>
    [Value("fa-rotate-by")]
    RotateBy = 7,
}
