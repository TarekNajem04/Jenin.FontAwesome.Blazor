//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Options for the <see cref="Float"/> animation.
/// </summary>
public sealed record FloatOptions : AnimationOptions {
    /// <summary>
    /// The horizontal drift distance applied while floating (any valid CSS length, for example <c>1px</c>).
    /// </summary>
    public string? Drift { get; init; }

    /// <summary>
    /// The max vertical float distance (any valid CSS length, for example <c>6px</c>).
    /// </summary>
    public string? Height { get; init; }

    /// <summary>
    /// The horizontal squash factor at the bottom of the float.
    /// </summary>
    public float? SquashX { get; init; }

    /// <summary>
    /// The vertical squash factor at the bottom of the float.
    /// </summary>
    public float? SquashY { get; init; }

    /// <summary>
    /// The horizontal stretch factor at the top of the float.
    /// </summary>
    public float? StretchX { get; init; }

    /// <summary>
    /// The vertical stretch factor at the top of the float.
    /// </summary>
    public float? StretchY { get; init; }

    /// <summary>
    /// The tilt angle of the float, in degrees.
    /// </summary>
    public float? Tilt { get; init; }
}
