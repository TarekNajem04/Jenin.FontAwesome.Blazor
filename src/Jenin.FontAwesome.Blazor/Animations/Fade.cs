//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// To fade an icon in and out visually to grab attention in a subtle (or not so subtle) way.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/animate#fade">Font Awesome Fade</a>.
/// </para>
/// </summary>
public sealed record Fade : Animation<Fade, FadeOptions> {
    /// <summary>
    /// Initializes a new instance of the <see cref="Fade"/> animation.
    /// </summary>
    /// <param name="options">Optional options; when <see langword="null"/> Font Awesome defaults are used.</param>
    public Fade(FadeOptions? options = null) : base(options) { }

    /// <summary>
    /// The Font Awesome CSS class that activates the <see cref="Fade"/> animation (<c>fa-fade</c>).
    /// </summary>
    protected override string CssClass => "fa-fade";

    /// <summary>
    /// Returns a copy of this <see cref="Fade"/> animation with the specified options.
    /// </summary>
    /// <param name="options">The options to apply.</param>
    /// <returns>A new <see cref="Fade"/> instance carrying <paramref name="options"/>.</returns>
    protected override Fade WithOptions(FadeOptions options) => this with { Options = options };

    /// <summary>
    /// Adds the <see cref="Fade"/> animation's CSS custom properties to <paramref name="styles"/>.
    /// </summary>
    /// <param name="styles">The CSS custom property bag to populate.</param>
    protected override void AddStyles(Dictionary<string, string> styles)
        => styles.AddOrUpdateIfNotNull("--fa-fade-opacity", Options.Opacity);
}
