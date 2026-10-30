//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor;

/// <summary>
/// The Font Awesome icon size scale, mapping to the <c>fa-2xs</c> through <c>fa-10x</c> sizing classes
/// as well as the relative <c>fa-xs</c>, <c>fa-sm</c>, <c>fa-lg</c>, <c>fa-xl</c>, and <c>fa-2xl</c> sizes.
/// </summary>
public enum IconSize {
    /// <summary>
    /// The icon size will not be applied.
    /// </summary>
    [Value("")]
    NotSet = 0,

    /// <summary>
    /// Inherit value from container.
    /// </summary>
    [Value("")]
    Inherits = 1,

    /// <summary>
    /// Changes an icon's font-size to 1em
    /// </summary>
    [Value("fa-1x")]
    X01 = 2,

    /// <summary>
    /// Changes an icon's font-size to 2em
    /// </summary>
    [Value("fa-2x")]
    X02 = 3,

    /// <summary>
    /// Changes an icon's font-size to 3em
    /// </summary>
    [Value("fa-3x")]
    X03 = 4,

    /// <summary>
    /// Changes an icon's font-size to 4em
    /// </summary>
    [Value("fa-4x")]
    X04 = 5,

    /// <summary>
    /// Changes an icon's font-size to 5em
    /// </summary>
    [Value("fa-5x")]
    X05 = 6,

    /// <summary>
    /// Changes an icon's font-size to 6em
    /// </summary>
    [Value("fa-6x")]
    X06 = 7,

    /// <summary>
    /// Changes an icon's font-size to 7em
    /// </summary>
    [Value("fa-7x")]
    X07 = 8,

    /// <summary>
    /// Changes an icon's font-size to 8em
    /// </summary>
    [Value("fa-8x")]
    X08 = 9,

    /// <summary>
    /// Changes an icon's font-size to 9em
    /// </summary>
    [Value("fa-9x")]
    X09 = 10,

    /// <summary>
    /// Changes an icon's font-size to 10em
    /// </summary>
    [Value("fa-10x")]
    X10 = 11,

    /// <summary>
    /// Changes an icon's font-size to 0.625em (~10px) and also vertically aligns icon
    /// </summary>
    [Value("fa-2xs")]
    ExtraSmall_2 = 12,

    /// <summary>
    /// Changes an icon's font-size to .75em (~12px) and also vertically aligns icon
    /// </summary>
    [Value("fa-xs")]
    ExtraSmall = 13,

    /// <summary>
    /// Changes an icon's font-size to 0.875em (~14px) and also vertically aligns icon
    /// </summary>
    [Value("fa-sm")]
    Small = 14,

    /// <summary>
    /// Changes an icon's font-size to 1.25em (~120px) and also vertically aligns icon
    /// </summary>
    [Value("fa-lg")]
    Large = 15,

    /// <summary>
    /// Changes an icon's font-size to 1.5em (~24px) and also vertically aligns icon
    /// </summary>
    [Value("fa-xl")]
    ExtraLarge = 16,

    /// <summary>
    /// Changes an icon's font-size to 2em (~32px) and also vertically aligns icon
    /// </summary>
    [Value("fa-2xl")]
    ExtraLarge_2 = 17,
}
