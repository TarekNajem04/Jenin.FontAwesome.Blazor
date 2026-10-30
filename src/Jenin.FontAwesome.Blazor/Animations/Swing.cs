//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Use this animation to swing an icon back and forth from its top center.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/animate#swing">Font Awesome Swing</a>.
/// </para>
/// </summary>
public sealed record Swing : Animation<Swing, SwingOptions> {
    /// <summary>
    /// Initializes a new instance of the <see cref="Swing"/> animation.
    /// </summary>
    /// <param name="options">Optional options; when <see langword="null"/> Font Awesome defaults are used.</param>
    public Swing(SwingOptions? options = null) : base(options) { }

    /// <summary>
    /// The Font Awesome CSS class that activates the <see cref="Swing"/> animation (<c>fa-swing</c>).
    /// </summary>
    protected override string CssClass => "fa-swing";

    /// <summary>
    /// Returns a copy of this <see cref="Swing"/> animation with the specified options.
    /// </summary>
    /// <param name="options">The options to apply.</param>
    /// <returns>A new <see cref="Swing"/> instance carrying <paramref name="options"/>.</returns>
    protected override Swing WithOptions(SwingOptions options) => this with { Options = options };

    /// <summary>
    /// Adds the <see cref="Swing"/> animation's CSS custom properties to <paramref name="styles"/>.
    /// </summary>
    /// <param name="styles">The CSS custom property bag to populate.</param>
    protected override void AddStyles(Dictionary<string, string> styles) {
        if(Options.Angle is { } angle) {
            _ = styles.AddOrUpdate("--fa-swing-angle", $"{angle.ToString("0.####", CultureInfo.InvariantCulture)}deg");
        }
    }
}
