//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Options for the <see cref="Jello"/> animation.
/// </summary>
public sealed record JelloOptions : AnimationOptions {
    /// <summary>
    /// The horizontal stretch factor reached at the sides of the wobble.
    /// </summary>
    public float? ScaleX { get; init; }

    /// <summary>
    /// The vertical stretch factor reached at the bottom of the wobble.
    /// </summary>
    public float? ScaleY { get; init; }
}
