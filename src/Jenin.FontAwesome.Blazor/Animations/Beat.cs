//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Use this animation to scale an icon up or down. This is useful for grabbing attention or for use with health/<c>heart-centric</c> icons.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/animate#beat">Font Awesome Beat</a>.
/// </para>
/// </summary>
public sealed record Beat : Animation<Beat, BeatOptions> {
    /// <summary>
    /// Initializes a new instance of the <see cref="Beat"/> animation.
    /// </summary>
    /// <param name="options">Optional options; when <see langword="null"/> Font Awesome defaults are used.</param>
    public Beat(BeatOptions? options = null) : base(options) { }

    /// <summary>
    /// The Font Awesome CSS class that activates the <see cref="Beat"/> animation (<c>fa-beat</c>).
    /// </summary>
    protected override string CssClass => "fa-beat";

    /// <summary>
    /// Returns a copy of this <see cref="Beat"/> animation with the specified options.
    /// </summary>
    /// <param name="options">The options to apply.</param>
    /// <returns>A new <see cref="Beat"/> instance carrying <paramref name="options"/>.</returns>
    protected override Beat WithOptions(BeatOptions options) => this with { Options = options };

    /// <summary>
    /// Adds the <see cref="Beat"/> animation's CSS custom properties to <paramref name="styles"/>.
    /// </summary>
    /// <param name="styles">The CSS custom property bag to populate.</param>
    protected override void AddStyles(Dictionary<string, string> styles)
        => styles.AddOrUpdateIfNotNull("--fa-beat-scale", Options.Scale);
}
