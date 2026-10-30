//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Use this animation to wobble an icon side to side like gelatin.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/animate#jello">Font Awesome Jello</a>.
/// </para>
/// </summary>
public sealed record Jello : Animation<Jello, JelloOptions> {
    /// <summary>
    /// Initializes a new instance of the <see cref="Jello"/> animation.
    /// </summary>
    /// <param name="options">Optional options; when <see langword="null"/> Font Awesome defaults are used.</param>
    public Jello(JelloOptions? options = null) : base(options) { }

    /// <summary>
    /// The Font Awesome CSS class that activates the <see cref="Jello"/> animation (<c>fa-jello</c>).
    /// </summary>
    protected override string CssClass => "fa-jello";

    /// <summary>
    /// Returns a copy of this <see cref="Jello"/> animation with the specified options.
    /// </summary>
    /// <param name="options">The options to apply.</param>
    /// <returns>A new <see cref="Jello"/> instance carrying <paramref name="options"/>.</returns>
    protected override Jello WithOptions(JelloOptions options) => this with { Options = options };

    /// <summary>
    /// Adds the <see cref="Jello"/> animation's CSS custom properties to <paramref name="styles"/>.
    /// </summary>
    /// <param name="styles">The CSS custom property bag to populate.</param>
    protected override void AddStyles(Dictionary<string, string> styles)
        => styles.AddOrUpdateIfNotNull("--fa-jello-scale-x", Options.ScaleX)
                 .AddOrUpdateIfNotNull("--fa-jello-scale-y", Options.ScaleY);
}
