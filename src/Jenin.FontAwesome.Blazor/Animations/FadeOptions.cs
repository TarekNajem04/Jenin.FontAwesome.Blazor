//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Options for the <see cref="Fade"/> animation.
/// </summary>
public sealed record FadeOptions : AnimationOptions {
    /// <summary>
    /// Lowest opacity value the Fade animation will fade to and from (normalized to the range 0 to 1).
    /// </summary>
    public float? Opacity {
        get;
        init => field = NormalizeUnitInterval(value);
    }
}
