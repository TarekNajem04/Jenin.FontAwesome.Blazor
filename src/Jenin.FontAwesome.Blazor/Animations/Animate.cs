//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// The single construction surface for Font Awesome animations.
/// <para>
/// Shared configuration flows through each animation's own options type (for example
/// <see cref="BounceOptions"/>); pass the typed options to select behavior. The options types
/// all derive from <see cref="AnimationOptions"/>.
/// </para>
/// </summary>
public static class Animate {
    /// <summary>
    /// Creates a new <see cref="Beat"/> animation.
    /// </summary>
    /// <param name="options">The animation options; when <see langword="null"/> Font Awesome defaults are used.</param>
    /// <returns>A new <see cref="Beat"/> animation instance.</returns>
    public static Beat Beat(BeatOptions? options = null) => new(options);

    /// <summary>
    /// Creates a new <see cref="Fade"/> animation.
    /// </summary>
    /// <param name="options">The animation options; when <see langword="null"/> Font Awesome defaults are used.</param>
    /// <returns>A new <see cref="Fade"/> animation instance.</returns>
    public static Fade Fade(FadeOptions? options = null) => new(options);

    /// <summary>
    /// Creates a new <see cref="BeatFade"/> animation.
    /// </summary>
    /// <param name="options">The animation options; when <see langword="null"/> Font Awesome defaults are used.</param>
    /// <returns>A new <see cref="BeatFade"/> animation instance.</returns>
    public static BeatFade BeatFade(BeatFadeOptions? options = null) => new(options);

    /// <summary>
    /// Creates a new <see cref="Bounce"/> animation.
    /// </summary>
    /// <param name="options">The animation options; when <see langword="null"/> Font Awesome defaults are used.</param>
    /// <returns>A new <see cref="Bounce"/> animation instance.</returns>
    public static Bounce Bounce(BounceOptions? options = null) => new(options);

    /// <summary>
    /// Creates a new <see cref="Flip"/> animation.
    /// </summary>
    /// <param name="options">The animation options; when <see langword="null"/> Font Awesome defaults are used.</param>
    /// <returns>A new <see cref="Flip"/> animation instance.</returns>
    public static Flip Flip(FlipOptions? options = null) => new(options);

    /// <summary>
    /// Creates a new <see cref="Shake"/> animation.
    /// </summary>
    /// <param name="options">The animation options; when <see langword="null"/> Font Awesome defaults are used.</param>
    /// <returns>A new <see cref="Shake"/> animation instance.</returns>
    public static Shake Shake(ShakeOptions? options = null) => new(options);

    /// <summary>
    /// Creates a new <see cref="Spin"/> animation.
    /// </summary>
    /// <param name="options">The animation options; when <see langword="null"/> Font Awesome defaults are used.</param>
    /// <returns>A new <see cref="Spin"/> animation instance.</returns>
    public static Spin Spin(SpinOptions? options = null) => new(options);

    /// <summary>
    /// Creates a new <see cref="SpinSnap"/> animation.
    /// </summary>
    /// <param name="options">The animation options; when <see langword="null"/> Font Awesome defaults are used.</param>
    /// <returns>A new <see cref="SpinSnap"/> animation instance.</returns>
    public static SpinSnap SpinSnap(SpinSnapOptions? options = null) => new(options);

    /// <summary>
    /// Creates a new <see cref="Buzz"/> animation.
    /// </summary>
    /// <param name="options">The animation options; when <see langword="null"/> Font Awesome defaults are used.</param>
    /// <returns>A new <see cref="Buzz"/> animation instance.</returns>
    public static Buzz Buzz(BuzzOptions? options = null) => new(options);

    /// <summary>
    /// Creates a new <see cref="Float"/> animation.
    /// </summary>
    /// <param name="options">The animation options; when <see langword="null"/> Font Awesome defaults are used.</param>
    /// <returns>A new <see cref="Float"/> animation instance.</returns>
    public static Float Float(FloatOptions? options = null) => new(options);

    /// <summary>
    /// Creates a new <see cref="Jello"/> animation.
    /// </summary>
    /// <param name="options">The animation options; when <see langword="null"/> Font Awesome defaults are used.</param>
    /// <returns>A new <see cref="Jello"/> animation instance.</returns>
    public static Jello Jello(JelloOptions? options = null) => new(options);

    /// <summary>
    /// Creates a new <see cref="Flip360"/> animation.
    /// </summary>
    /// <param name="options">The animation options; when <see langword="null"/> Font Awesome defaults are used.</param>
    /// <returns>A new <see cref="Flip360"/> animation instance.</returns>
    public static Flip360 Flip360(Flip360Options? options = null) => new(options);

    /// <summary>
    /// Creates a new <see cref="Swing"/> animation.
    /// </summary>
    /// <param name="options">The animation options; when <see langword="null"/> Font Awesome defaults are used.</param>
    /// <returns>A new <see cref="Swing"/> animation instance.</returns>
    public static Swing Swing(SwingOptions? options = null) => new(options);

    /// <summary>
    /// Creates a new <see cref="Wag"/> animation.
    /// </summary>
    /// <param name="options">The animation options; when <see langword="null"/> Font Awesome defaults are used.</param>
    /// <returns>A new <see cref="Wag"/> animation instance.</returns>
    public static Wag Wag(WagOptions? options = null) => new(options);
}
