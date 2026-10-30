//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Explorer.Snippets;

/// <summary>
/// Maps the library's presentation enums to their emitted Font Awesome class tokens.
/// </summary>
/// <remarks>
/// The library stores these tokens in an internal <c>ValueAttribute</c> (not visible outside the core
/// assembly), and the core public surface is frozen, so the explorer reproduces the mapping here. The
/// mapping only needs to cover the Free-relevant members the explorer exposes; unknown members fall
/// back to the enum name's value, which is never a valid class token and is therefore omitted.
/// </remarks>
internal static class FontAwesomeClassTokens {
    /// <summary>Returns the class token for <paramref name="style"/>, or <c>null</c> when none applies.</summary>
    public static string? For(IconStyle style) => style switch {
        IconStyle.Solid => "fa-solid",
        IconStyle.Regular => "fa-regular",
        IconStyle.Light => "fa-light",
        IconStyle.Thin => "fa-thin",
        IconStyle.Duotone => "fa-duotone fa-solid",
        IconStyle.Brands => "fa-brands",
        _ => null,
    };

    /// <summary>Returns the class token for <paramref name="typeface"/>, or <c>null</c> when none applies.</summary>
    public static string? For(Typeface typeface) => typeface switch {
        Typeface.Sharp => "fa-sharp",
        Typeface.Duotone => "fa-duotone",
        Typeface.SharpDuotone => "fa-sharp-duotone",
        _ => null,
    };

    /// <summary>Returns the class token for <paramref name="size"/>, or <c>null</c> when none applies.</summary>
    public static string? For(IconSize size) => size switch {
        IconSize.X01 => "fa-1x",
        IconSize.X02 => "fa-2x",
        IconSize.X03 => "fa-3x",
        IconSize.X04 => "fa-4x",
        IconSize.X05 => "fa-5x",
        IconSize.X06 => "fa-6x",
        IconSize.X07 => "fa-7x",
        IconSize.X08 => "fa-8x",
        IconSize.X09 => "fa-9x",
        IconSize.X10 => "fa-10x",
        IconSize.ExtraSmall_2 => "fa-2xs",
        IconSize.ExtraSmall => "fa-xs",
        IconSize.Small => "fa-sm",
        IconSize.Large => "fa-lg",
        IconSize.ExtraLarge => "fa-xl",
        IconSize.ExtraLarge_2 => "fa-2xl",
        _ => null,
    };

    /// <summary>Returns the class token for <paramref name="width"/>, or <c>null</c> when none applies.</summary>
    public static string? For(IconWidth width) => width switch {
        IconWidth.Auto => "fa-width-auto",
        IconWidth.Fixed => "fa-width-fixed",
        _ => null,
    };

    /// <summary>Returns the class token for <paramref name="rotate"/>, or <c>null</c> when none applies.</summary>
    public static string? For(IconRotate rotate) => rotate switch {
        IconRotate.Deg090 => "fa-rotate-90",
        IconRotate.Deg180 => "fa-rotate-180",
        IconRotate.Deg270 => "fa-rotate-270",
        IconRotate.FlipHorizontal => "fa-flip-horizontal",
        IconRotate.FlipVertical => "fa-flip-vertical",
        IconRotate.FlipBoth => "fa-flip-both",
        IconRotate.RotateBy => "fa-rotate-by",
        _ => null,
    };

    /// <summary>Returns the class token for <paramref name="pull"/>, or <c>null</c> when none applies.</summary>
    public static string? For(PullPosition pull) => pull switch {
        PullPosition.Left => "fa-pull-left",
        PullPosition.Right => "fa-pull-right",
        PullPosition.Start => "fa-pull-start",
        PullPosition.End => "fa-pull-end",
        _ => null,
    };

    /// <summary>Returns the class token for <paramref name="direction"/>, or <c>null</c> when none applies.</summary>
    public static string? For(AnimationDirection direction) => direction switch {
        AnimationDirection.Normal => "normal",
        AnimationDirection.Reverse => "reverse",
        AnimationDirection.Alternate => "alternate",
        AnimationDirection.AlternateReverse => "alternate-reverse",
        _ => null,
    };
}
