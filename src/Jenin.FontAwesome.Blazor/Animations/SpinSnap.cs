//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Animations;

/// <summary>
/// Use this animation to rotate an icon with a stepped "snap" motion. Select the step count
/// with <see cref="SpinSnapOptions.Size"/>.
/// <para>
/// <a href="https://fontawesome.com/docs/web/style/animate#spin-snap">Font Awesome Spin Snap</a>.
/// </para>
/// </summary>
public sealed record SpinSnap : Animation<SpinSnap, SpinSnapOptions> {
    /// <summary>
    /// Initializes a new instance of the <see cref="SpinSnap"/> animation.
    /// </summary>
    /// <param name="options">Optional options; when <see langword="null"/> Font Awesome defaults are used.</param>
    public SpinSnap(SpinSnapOptions? options = null) : base(options) { }

    /// <summary>
    /// The Font Awesome CSS class that activates this animation, selected by <see cref="SpinSnapOptions.Size"/>.
    /// </summary>
    protected override string CssClass => Options.Size switch {
        SpinSnapSize.Four => "fa-spin-snap-4",
        SpinSnapSize.Eight => "fa-spin-snap-8",
        _ => "fa-spin-snap"
    };

    /// <summary>
    /// Returns a copy of this <see cref="SpinSnap"/> animation with the specified options.
    /// </summary>
    /// <param name="options">The options to apply.</param>
    /// <returns>A new <see cref="SpinSnap"/> instance carrying <paramref name="options"/>.</returns>
    protected override SpinSnap WithOptions(SpinSnapOptions options) => this with { Options = options };
}
