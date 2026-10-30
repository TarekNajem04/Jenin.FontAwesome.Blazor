//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Use this animation to get any icon to rotate, and use <see cref="SpinOptions.Pulse"/> to have it rotate with eight steps.
/// This works especially well with spinner icons.
/// If you would like an icon to spin in reverse (e.g., counter-clockwise), set the <see cref="SpinOptions.Reverse"/> property.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/animate#spin">Font Awesome Spin</a>.
/// </para>
/// </summary>
public sealed record Spin : Animation<Spin, SpinOptions> {
    /// <summary>
    /// Initializes a new instance of the <see cref="Spin"/> animation.
    /// </summary>
    /// <param name="options">Optional options; when <see langword="null"/> Font Awesome defaults are used.</param>
    public Spin(SpinOptions? options = null) : base(options) { }

    /// <summary>
    /// The Font Awesome CSS class that activates the <see cref="Spin"/> animation (<c>fa-spin</c>).
    /// </summary>
    protected override string CssClass => "fa-spin";

    /// <summary>
    /// Returns a copy of this <see cref="Spin"/> animation with the specified options.
    /// </summary>
    /// <param name="options">The options to apply.</param>
    /// <returns>A new <see cref="Spin"/> instance carrying <paramref name="options"/>.</returns>
    protected override Spin WithOptions(SpinOptions options) => this with { Options = options };

    /// <summary>
    /// Builds the Font Awesome animation CSS classes, including the pulse and reverse variants.
    /// </summary>
    /// <returns>The space-delimited animation CSS classes.</returns>
    public override string BuildClasses()
        => string.Join
                  (
                    ' ',
                    new[] {
                        Options.Pulse == true ? "fa-spin-pulse" : base.BuildClasses(),
                        Options.Reverse == true ? "fa-spin-reverse" : null
                    }
                    .Where(x => !string.IsNullOrEmpty(x))
                  )
                 .Trim();
}
