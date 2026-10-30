//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Use this animation to grab attention by visually bouncing an icon up and down.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/animate#bounce">Font Awesome Bounce</a>.
/// </para>
/// </summary>
public sealed record Bounce : Animation<Bounce, BounceOptions> {
    /// <summary>
    /// Initializes a new instance of the <see cref="Bounce"/> animation.
    /// </summary>
    /// <param name="options">Optional options; when <see langword="null"/> Font Awesome defaults are used.</param>
    public Bounce(BounceOptions? options = null) : base(options) { }

    /// <summary>
    /// The Font Awesome CSS class that activates the <see cref="Bounce"/> animation (<c>fa-bounce</c>).
    /// </summary>
    protected override string CssClass => "fa-bounce";

    /// <summary>
    /// Returns a copy of this <see cref="Bounce"/> animation with the specified options.
    /// </summary>
    /// <param name="options">The options to apply.</param>
    /// <returns>A new <see cref="Bounce"/> instance carrying <paramref name="options"/>.</returns>
    protected override Bounce WithOptions(BounceOptions options) => this with { Options = options };

    /// <summary>
    /// Adds the <see cref="Bounce"/> animation's CSS custom properties to <paramref name="styles"/>.
    /// </summary>
    /// <param name="styles">The CSS custom property bag to populate.</param>
    protected override void AddStyles(Dictionary<string, string> styles)
        => styles.AddOrUpdateIfNotNull("--fa-bounce-rebound", Options.Rebound)
                  .AddOrUpdateIfNotNull("--fa-bounce-height", Options.Height)
                  .AddOrUpdateIfNotNull("--fa-bounce-start-scale-x", Options.StartScaleX)
                  .AddOrUpdateIfNotNull("--fa-bounce-start-scale-y", Options.StartScaleY)
                  .AddOrUpdateIfNotNull("--fa-bounce-jump-scale-x", Options.JumpScaleX)
                  .AddOrUpdateIfNotNull("--fa-bounce-jump-scale-y", Options.JumpScaleY)
                  .AddOrUpdateIfNotNull("--fa-bounce-land-scale-x", Options.LandScaleX)
                  .AddOrUpdateIfNotNull("--fa-bounce-land-scale-y", Options.LandScaleY)
                  .AddOrUpdateIfNotNull("--fa-bounce-anticipation", Options.Anticipation);
}
