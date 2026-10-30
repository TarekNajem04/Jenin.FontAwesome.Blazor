//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Use this animation to wag an icon back and forth from its bottom center.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/animate#wag">Font Awesome Wag</a>.
/// </para>
/// </summary>
public sealed record Wag : Animation<Wag, WagOptions> {
    /// <summary>
    /// Initializes a new instance of the <see cref="Wag"/> animation.
    /// </summary>
    /// <param name="options">Optional options; when <see langword="null"/> Font Awesome defaults are used.</param>
    public Wag(WagOptions? options = null) : base(options) { }

    /// <summary>
    /// The Font Awesome CSS class that activates the <see cref="Wag"/> animation (<c>fa-wag</c>).
    /// </summary>
    protected override string CssClass => "fa-wag";

    /// <summary>
    /// Returns a copy of this <see cref="Wag"/> animation with the specified options.
    /// </summary>
    /// <param name="options">The options to apply.</param>
    /// <returns>A new <see cref="Wag"/> instance carrying <paramref name="options"/>.</returns>
    protected override Wag WithOptions(WagOptions options) => this with { Options = options };

    /// <summary>
    /// Adds the <see cref="Wag"/> animation's CSS custom properties to <paramref name="styles"/>.
    /// </summary>
    /// <param name="styles">The CSS custom property bag to populate.</param>
    protected override void AddStyles(Dictionary<string, string> styles) {
        if(Options.Angle is { } angle) {
            _ = styles.AddOrUpdate("--fa-wag-angle", $"{angle.ToString("0.####", CultureInfo.InvariantCulture)}deg");
        }
    }
}
