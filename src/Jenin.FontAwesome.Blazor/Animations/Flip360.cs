//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Use this animation to continuously rotate an icon a full 360 degrees about its Y axis.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/animate#flip-360">Font Awesome Flip 360</a>.
/// </para>
/// </summary>
public sealed record Flip360 : Animation<Flip360, Flip360Options> {
    /// <summary>
    /// Initializes a new instance of the <see cref="Flip360"/> animation.
    /// </summary>
    /// <param name="options">Optional options; when <see langword="null"/> Font Awesome defaults are used.</param>
    public Flip360(Flip360Options? options = null) : base(options) { }

    /// <summary>
    /// The Font Awesome CSS class that activates the <see cref="Flip360"/> animation (<c>fa-flip-360</c>).
    /// </summary>
    protected override string CssClass => "fa-flip-360";

    /// <summary>
    /// Returns a copy of this <see cref="Flip360"/> animation with the specified options.
    /// </summary>
    /// <param name="options">The options to apply.</param>
    /// <returns>A new <see cref="Flip360"/> instance carrying <paramref name="options"/>.</returns>
    protected override Flip360 WithOptions(Flip360Options options) => this with { Options = options };
}
