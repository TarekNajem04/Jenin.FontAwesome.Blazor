//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Base immutable value object for Font Awesome animations.
/// <para>
/// An animation carries its identity (the Font Awesome animation class), its
/// <see cref="Options"/>, and pure serialization. Serialization never mutates the instance.
/// </para>
/// </summary>
public abstract record Animation {
    /// <summary>
    /// Initializes a new instance of the <see cref="Animation"/> animation.
    /// </summary>
    /// <param name="options">The shared animation options.</param>
    protected Animation(AnimationOptions options) => Options = options;

    /// <summary>
    /// Configuration for this animation. Concrete animations narrow this to their own options type.
    /// </summary>
    public AnimationOptions Options { get; init; }

    /// <summary>
    /// The Font Awesome base animation class for this animation type.
    /// </summary>
    protected abstract string CssClass { get; }

    /// <summary>
    /// Builds the Font Awesome animation CSS classes. Pure and deterministic.
    /// <returns>The Font Awesome animation CSS classes for this animation.</returns>
    /// </summary>
    public virtual string BuildClasses() => CssClass;

    /// <summary>
    /// Builds the Font Awesome animation CSS custom properties. Pure and deterministic;
    /// repeated calls yield the same result and never mutate this instance.
    /// <returns>The CSS custom properties (name/value pairs) emitted for this animation.</returns>
    /// </summary>
    public IReadOnlyDictionary<string, string> BuildStyles() {
        var styles = new Dictionary<string, string>();

        AddOptionsStyles(styles);
        AddStyles(styles);

        return styles;
    }

    /// <summary>
    /// Serialization hook for animation-type specific CSS custom properties.
    /// Implementations must not mutate the animation state.
    /// <param name="styles">The CSS custom property bag to populate.</param>
    /// </summary>
    protected virtual void AddStyles(Dictionary<string, string> styles) { }

    private void AddOptionsStyles(Dictionary<string, string> styles) {
        var options = Options;

        if(options.Delay.HasValue) {
            _ = styles.AddOrUpdate("--fa-animation-delay", FormatTime(options.Delay.Value));
        }

        if(options.Direction is not AnimationDirection.None) {
            _ = styles.AddOrUpdateIfNotNull("--fa-animation-direction", options.Direction.GetStringValue());
        }

        if(options.Duration.HasValue) {
            _ = styles.AddOrUpdate("--fa-animation-duration", FormatTime(options.Duration.Value));
        }

        _ = styles.AddOrUpdateIfNotNull("--fa-animation-iteration-count", options.IterationCount)
                  .AddOrUpdateIfNotNull("--fa-animation-timing", options.TimingFunction);
    }

    private static string FormatTime(TimeSpan value)
        => value.TotalMilliseconds.ToString("0.####", CultureInfo.InvariantCulture) + "ms";
}

/// <summary>
/// Strongly-typed fluent surface shared by every concrete <see cref="Animation"/>.
/// <para>
/// Setters return the concrete animation type (<typeparamref name="TSelf"/>) so configuration
/// chains preserve the type and never fall back to the non-generic base. The shared settings are
/// carried by <typeparamref name="TOptions"/>, the animation's own options type. The non-generic
/// <see cref="Animation"/> remains the parameter contract (for example <c>Icon.Animation</c>).
/// </para>
/// </summary>
/// <typeparam name="TSelf">The concrete animation type providing its own projection.</typeparam>
/// <typeparam name="TOptions">The concrete animation's options type deriving from <see cref="AnimationOptions"/>.</typeparam>
public abstract record Animation<TSelf, TOptions> : Animation
    where TSelf : Animation<TSelf, TOptions>
    where TOptions : AnimationOptions, new() {
    /// <summary>
    /// Initializes a new instance of the <see cref="Animation{TSelf, TOptions}"/> animation.
    /// </summary>
    /// <param name="options">The animation options; when <see langword="null"/> default options are used.</param>
    protected Animation(TOptions? options = null) : base(options ?? new TOptions()) { }

    /// <summary>
    /// Configuration for this animation, narrowed to its own options type.
    /// </summary>
    public new TOptions Options {
        get => (TOptions)base.Options;
        init => base.Options = value;
    }

    /// <summary>
    /// Projects this instance onto a new instance of the concrete type with the supplied options.
    /// <param name="options">The options to carry on the new instance.</param>
    /// <returns>A new instance of the concrete animation type carrying the supplied options.</returns>
    /// </summary>
    protected abstract TSelf WithOptions(TOptions options);

    /// <summary>
    /// Sets the initial delay, returning a new instance of the concrete animation type.
    /// <param name="delay">The initial delay before the animation starts.</param>
    /// <returns>A new instance of the concrete animation type with the delay applied.</returns>
    /// </summary>
    public TSelf SetDelay(TimeSpan delay) => WithOptions(Options with { Delay = delay });

    /// <summary>
    /// Sets the cycle duration, returning a new instance of the concrete animation type.
    /// <param name="duration">The duration of a single animation cycle.</param>
    /// <returns>A new instance of the concrete animation type with the duration applied.</returns>
    /// </summary>
    public TSelf SetDuration(TimeSpan duration) => WithOptions(Options with { Duration = duration });

    /// <summary>
    /// Sets the playback direction, returning a new instance of the concrete animation type.
    /// <param name="direction">The playback direction.</param>
    /// <returns>A new instance of the concrete animation type with the direction applied.</returns>
    /// </summary>
    public TSelf SetDirection(AnimationDirection direction) => WithOptions(Options with { Direction = direction });

    /// <summary>
    /// Sets the iteration count, returning a new instance of the concrete animation type.
    /// <param name="iterationCount">The number of times the animation plays before stopping.</param>
    /// <returns>A new instance of the concrete animation type with the iteration count applied.</returns>
    /// </summary>
    public TSelf SetIterationCount(float? iterationCount) => WithOptions(Options with { IterationCount = iterationCount });

    /// <summary>
    /// Sets the timing function, returning a new instance of the concrete animation type.
    /// <param name="timingFunction">Any valid CSS <c>animation-timing-function</c> value.</param>
    /// <returns>A new instance of the concrete animation type with the timing function applied.</returns>
    /// </summary>
    public TSelf SetTimingFunction(string timingFunction) => WithOptions(Options with { TimingFunction = timingFunction });
}
