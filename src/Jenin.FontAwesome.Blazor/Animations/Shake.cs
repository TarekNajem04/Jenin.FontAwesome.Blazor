//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Use this animation to grab attention or note that something is not allowed by shaking an icon back and forth.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/animate#shake">Font Awesome Shake</a>.
/// </para>
/// </summary>
public sealed record Shake : Animation<Shake, ShakeOptions> {
    /// <summary>
    /// Initializes a new instance of the <see cref="Shake"/> animation.
    /// </summary>
    /// <param name="options">Optional options; when <see langword="null"/> Font Awesome defaults are used.</param>
    public Shake(ShakeOptions? options = null) : base(options) { }

    /// <summary>
    /// The Font Awesome CSS class that activates the <see cref="Shake"/> animation (<c>fa-shake</c>).
    /// </summary>
    protected override string CssClass => "fa-shake";

    /// <summary>
    /// Returns a copy of this <see cref="Shake"/> animation with the specified options.
    /// </summary>
    /// <param name="options">The options to apply.</param>
    /// <returns>A new <see cref="Shake"/> instance carrying <paramref name="options"/>.</returns>
    protected override Shake WithOptions(ShakeOptions options) => this with { Options = options };
}
