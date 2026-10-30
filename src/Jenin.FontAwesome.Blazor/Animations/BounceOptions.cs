//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Options for the <see cref="Bounce"/> animation.
/// </summary>
public sealed record BounceOptions : AnimationOptions {
    /// <summary>
    /// The amount of rebound an icon has when landing after the jump.
    /// </summary>
    public float? Rebound { get; init; }

    /// <summary>
    /// The max height an icon will jump to when bouncing.
    /// </summary>
    public float? Height { get; init; }

    /// <summary>
    /// The icon's horizontal distortion ("squish") when starting to bounce.
    /// </summary>
    public float? StartScaleX { get; init; }

    /// <summary>
    /// The icon's vertical distortion ("squish") when starting to bounce.
    /// </summary>
    public float? StartScaleY { get; init; }

    /// <summary>
    /// The icon's horizontal distortion ("squish") at the top of the jump.
    /// </summary>
    public float? JumpScaleX { get; init; }

    /// <summary>
    /// The icon's vertical distortion ("squish") at the top of the jump.
    /// </summary>
    public float? JumpScaleY { get; init; }

    /// <summary>
    /// The icon's horizontal distortion ("squish") when landing after the jump.
    /// </summary>
    public float? LandScaleX { get; init; }

    /// <summary>
    /// The vertical distortion ("squish") when landing after the jump.
    /// </summary>
    public float? LandScaleY { get; init; }

    /// <summary>
    /// The vertical anticipation offset applied before the first jump (any valid CSS length, for example <c>3px</c>).
    /// </summary>
    public string? Anticipation { get; init; }
}
