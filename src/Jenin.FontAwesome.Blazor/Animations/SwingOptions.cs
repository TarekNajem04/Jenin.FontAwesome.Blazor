//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Options for the <see cref="Swing"/> animation.
/// </summary>
public sealed record SwingOptions : AnimationOptions {
    /// <summary>
    /// The swing angle, in degrees.
    /// </summary>
    public float? Angle { get; init; }
}
