//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Use this animation to rotate an icon in 3D space.
/// By default, flip rotates an icon about the Y axis 180 degrees.
/// Flipping is helpful for transitions, processing states,
/// or for using physical objects that one flips in the real world.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/animate#flip">Font Awesome Flip</a>.
/// </para>
/// </summary>
public sealed record Flip : Animation<Flip, FlipOptions> {
    /// <summary>
    /// Initializes a new instance of the <see cref="Flip"/> animation.
    /// </summary>
    /// <param name="options">Optional options; when <see langword="null"/> Font Awesome defaults are used.</param>
    public Flip(FlipOptions? options = null) : base(options) { }

    /// <summary>
    /// The Font Awesome CSS class that activates the <see cref="Flip"/> animation (<c>fa-flip</c>).
    /// </summary>
    protected override string CssClass => "fa-flip";

    /// <summary>
    /// Returns a copy of this <see cref="Flip"/> animation with the specified options.
    /// </summary>
    /// <param name="options">The options to apply.</param>
    /// <returns>A new <see cref="Flip"/> instance carrying <paramref name="options"/>.</returns>
    protected override Flip WithOptions(FlipOptions options) => this with { Options = options };

    /// <summary>
    /// Adds the <see cref="Flip"/> animation's CSS custom properties to <paramref name="styles"/>.
    /// </summary>
    /// <param name="styles">The CSS custom property bag to populate.</param>
    protected override void AddStyles(Dictionary<string, string> styles)
        => styles.AddOrUpdateIfNotNull("--fa-flip-x", Options.FlipX)
                  .AddOrUpdateIfNotNull("--fa-flip-y", Options.FlipY)
                  .AddOrUpdateIfNotNull("--fa-flip-z", Options.FlipZ)
                  .AddOrUpdateIfNotNull("--fa-flip-angle", Options.FlipAngle)
                  .AddOrUpdateIfNotNull("--fa-flip-overshoot", Options.Overshoot)
                  .AddOrUpdateIfNotNull("--fa-flip-anticipation-scale", Options.AnticipationScale);
}
