//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Use this animation to grab attention with a quick horizontal buzz (vibration) of an icon.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/animate#buzz">Font Awesome Buzz</a>.
/// </para>
/// </summary>
public sealed record Buzz : Animation<Buzz, BuzzOptions> {
    /// <summary>
    /// Initializes a new instance of the <see cref="Buzz"/> animation.
    /// </summary>
    /// <param name="options">Optional options; when <see langword="null"/> Font Awesome defaults are used.</param>
    public Buzz(BuzzOptions? options = null) : base(options) { }

    /// <summary>
    /// The Font Awesome CSS class that activates the <see cref="Buzz"/> animation (<c>fa-buzz</c>).
    /// </summary>
    protected override string CssClass => "fa-buzz";

    /// <summary>
    /// Returns a copy of this <see cref="Buzz"/> animation with the specified options.
    /// </summary>
    /// <param name="options">The options to apply.</param>
    /// <returns>A new <see cref="Buzz"/> instance carrying <paramref name="options"/>.</returns>
    protected override Buzz WithOptions(BuzzOptions options) => this with { Options = options };

    /// <summary>
    /// Adds the <see cref="Buzz"/> animation's CSS custom properties to <paramref name="styles"/>.
    /// </summary>
    /// <param name="styles">The CSS custom property bag to populate.</param>
    protected override void AddStyles(Dictionary<string, string> styles)
        => styles.AddOrUpdateIfNotNull("--fa-buzz-distance", Options.Distance);
}
