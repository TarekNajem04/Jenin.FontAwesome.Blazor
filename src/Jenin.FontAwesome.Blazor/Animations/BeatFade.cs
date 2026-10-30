//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Use this animation to grab attention by visually scaling and pulsing an icon in and out.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/animate#beat-fade">Font Awesome Beat Fade</a>.
/// </para>
/// </summary>
public sealed record BeatFade : Animation<BeatFade, BeatFadeOptions> {
    /// <summary>
    /// Initializes a new instance of the <see cref="BeatFade"/> animation.
    /// </summary>
    /// <param name="options">Optional options; when <see langword="null"/> Font Awesome defaults are used.</param>
    public BeatFade(BeatFadeOptions? options = null) : base(options) { }

    /// <summary>
    /// The Font Awesome CSS class that activates the <see cref="BeatFade"/> animation (<c>fa-beat-fade</c>).
    /// </summary>
    protected override string CssClass => "fa-beat-fade";

    /// <summary>
    /// Returns a copy of this <see cref="BeatFade"/> animation with the specified options.
    /// </summary>
    /// <param name="options">The options to apply.</param>
    /// <returns>A new <see cref="BeatFade"/> instance carrying <paramref name="options"/>.</returns>
    protected override BeatFade WithOptions(BeatFadeOptions options) => this with { Options = options };

    /// <summary>
    /// Adds the <see cref="BeatFade"/> animation's CSS custom properties to <paramref name="styles"/>.
    /// </summary>
    /// <param name="styles">The CSS custom property bag to populate.</param>
    protected override void AddStyles(Dictionary<string, string> styles)
        => styles.AddOrUpdateIfNotNull("--fa-beat-fade-opacity", Options.Opacity)
                  .AddOrUpdateIfNotNull("--fa-beat-fade-scale", Options.Scale);
}
