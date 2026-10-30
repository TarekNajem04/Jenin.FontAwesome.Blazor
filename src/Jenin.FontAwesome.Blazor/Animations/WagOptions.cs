//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Options for the <see cref="Wag"/> animation.
/// </summary>
public sealed record WagOptions : AnimationOptions {
    /// <summary>
    /// The wag angle, in degrees.
    /// </summary>
    public float? Angle { get; init; }
}
