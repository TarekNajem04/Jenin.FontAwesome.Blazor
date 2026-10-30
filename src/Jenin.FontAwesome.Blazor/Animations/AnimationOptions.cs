//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Base, immutable configuration shared by every <see cref="Animation"/>.
/// <para>
/// Each concrete animation exposes its own options type (for example <see cref="BounceOptions"/>)
/// deriving from this base, so shared settings live here and animation-specific settings live on the
/// derived options. Values are normalized at construction; serialization never mutates.
/// </para>
/// </summary>
public abstract record AnimationOptions {
    /// <summary>
    /// Initial delay before the animation starts. <see langword="null"/> emits nothing.
    /// </summary>
    public TimeSpan? Delay { get; init; }

    /// <summary>
    /// Duration of a single animation cycle. <see langword="null"/> emits nothing.
    /// </summary>
    public TimeSpan? Duration { get; init; }

    /// <summary>
    /// Direction in which the animation is played. <see cref="AnimationDirection.None"/> emits nothing.
    /// </summary>
    public AnimationDirection Direction { get; init; }

    /// <summary>
    /// Number of times the animation plays before stopping (normalized to be non-negative).
    /// <see langword="null"/> emits nothing, leaving Font Awesome's default (<c>infinite</c>).
    /// </summary>
    public float? IterationCount {
        get;
        init => field = value.HasValue ? MathF.Max(0, value.Value) : value;
    }

    /// <summary>
    /// Any valid CSS <c>animation-timing-function</c> value. <see langword="null"/> emits nothing.
    /// </summary>
    public string? TimingFunction { get; init; }

    /// <summary>
    /// Clamps an optional opacity/vector component to the inclusive range 0 to 1.
    /// <param name="value">The optional component to clamp; <see langword="null"/> is passed through.</param>
    /// <returns>The value clamped to the inclusive range 0 to 1, or <see langword="null"/> when <paramref name="value"/> is <see langword="null"/>.</returns>
    /// </summary>
    protected static float? NormalizeUnitInterval(float? value)
        => value.HasValue ? MathF.Min(1, MathF.Max(0, value.Value)) : value;
}
