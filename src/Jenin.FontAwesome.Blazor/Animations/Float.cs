//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Use this animation to gently float an icon up and down for a calm, ambient effect.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/animate#float">Font Awesome Float</a>.
/// </para>
/// </summary>
public sealed record Float : Animation<Float, FloatOptions> {
    /// <summary>
    /// Initializes a new instance of the <see cref="Float"/> animation.
    /// </summary>
    /// <param name="options">Optional options; when <see langword="null"/> Font Awesome defaults are used.</param>
    public Float(FloatOptions? options = null) : base(options) { }

    /// <summary>
    /// The Font Awesome CSS class that activates the <see cref="Float"/> animation (<c>fa-float</c>).
    /// </summary>
    protected override string CssClass => "fa-float";

    /// <summary>
    /// Returns a copy of this <see cref="Float"/> animation with the specified options.
    /// </summary>
    /// <param name="options">The options to apply.</param>
    /// <returns>A new <see cref="Float"/> instance carrying <paramref name="options"/>.</returns>
    protected override Float WithOptions(FloatOptions options) => this with { Options = options };

    /// <summary>
    /// Adds the <see cref="Float"/> animation's CSS custom properties to <paramref name="styles"/>.
    /// </summary>
    /// <param name="styles">The CSS custom property bag to populate.</param>
    protected override void AddStyles(Dictionary<string, string> styles) {
        _ = styles.AddOrUpdateIfNotNull("--fa-float-drift", Options.Drift)
                  .AddOrUpdateIfNotNull("--fa-float-height", Options.Height)
                  .AddOrUpdateIfNotNull("--fa-float-squash-x", Options.SquashX)
                  .AddOrUpdateIfNotNull("--fa-float-squash-y", Options.SquashY)
                  .AddOrUpdateIfNotNull("--fa-float-stretch-x", Options.StretchX)
                  .AddOrUpdateIfNotNull("--fa-float-stretch-y", Options.StretchY);

        if(Options.Tilt is { } tilt) {
            _ = styles.AddOrUpdate("--fa-float-tilt", $"{tilt.ToString("0.####", CultureInfo.InvariantCulture)}deg");
        }
    }
}
