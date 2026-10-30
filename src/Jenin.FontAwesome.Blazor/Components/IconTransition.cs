//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// Crossfades (or scales, flips, slides) between two content states whenever
/// <see cref="ShowAlternate"/> changes. Both states render stacked in one grid cell, so the
/// component keeps the largest state's size and never shifts layout.
/// </summary>
/// <remarks>
/// <para>
/// Layers are stacked with CSS grid (no JavaScript); visibility flips with the transition, so
/// hidden interactive content is neither focusable nor announced. Everything collapses to an
/// instant swap under <c>prefers-reduced-motion</c> (see <c>animation-triggers.css</c>).
/// </para>
/// </remarks>
public class IconTransition : Element {
    /// <summary>
    /// The first (default) state's content.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// The alternate state's content.
    /// </summary>
    [Parameter]
    public RenderFragment? AlternateContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the transition's alternate state is shown.
    /// </summary>
    [Parameter]
    public bool ShowAlternate { get; set; }

    /// <summary>
    /// The transition played when the transition state changes.
    /// </summary>
    [Parameter]
    public IconTransitionKind Transition { get; set; } = IconTransitionKind.Crossfade;

    /// <summary>
    /// The duration of the transition. <c>null</c> uses the stylesheet default (0.25s).
    /// </summary>
    [Parameter]
    public TimeSpan? Duration { get; set; }

    private protected override Dictionary<string, string> AddStyles(Dictionary<string, string> styles) {
        styles = base.AddStyles(styles);

        styles["display"] = "inline-grid";

        if(Duration.HasValue) {
            styles["--jfa-transition-duration"] = Duration.Value.TotalMilliseconds.ToString("0.####", CultureInfo.InvariantCulture) + "ms";
        }

        return styles;
    }

    private protected override void BuildAccessibilityAttributes() {
        AdditionalAttributes ??= [];

        _ = AdditionalAttributes.SetOrRemoveIfEmpty(
            "class",
            string.Join(
                ' ',
                new[] {
                    "jfa-anim-transition",
                    Convert.ToString(AdditionalAttributes.TryGetValue("class", out var existing) ? existing : null, CultureInfo.InvariantCulture)?.Replace("jfa-anim-transition", string.Empty, StringComparison.Ordinal),
                }.Where(static part => !string.IsNullOrWhiteSpace(part))));

        base.BuildAccessibilityAttributes();
    }

    private static string LayerStyle(bool visible, string hiddenTransform, string shownTransform) =>
        $"opacity:{(visible ? 1 : 0)};transform:{(visible ? shownTransform : hiddenTransform)};visibility:{(visible ? "visible" : "hidden")};";

    private (string First, string Second) LayerStyles => Transition switch {
        IconTransitionKind.Scale =>
            (LayerStyle(!ShowAlternate, "scale(0.2)", "scale(1)"),
             LayerStyle(ShowAlternate, "scale(0.2)", "scale(1)")),
        IconTransitionKind.Flip =>
            (LayerStyle(!ShowAlternate, "rotateY(89deg)", "rotateY(0deg)"),
             LayerStyle(ShowAlternate, "rotateY(-89deg)", "rotateY(0deg)")),
        IconTransitionKind.Slide =>
            (LayerStyle(!ShowAlternate, "translateX(-35%)", "translateX(0)"),
             LayerStyle(ShowAlternate, "translateX(35%)", "translateX(0)")),
        _ =>
            (LayerStyle(!ShowAlternate, "none", "none"),
             LayerStyle(ShowAlternate, "none", "none")),
    };

    /// <summary>
    /// Builds the component's render tree: two stacked layers whose inline opacity, transform,
    /// and visibility reflect the current state.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        var (first, second) = LayerStyles;

        builder.OpenElement(0, "span");
        AddAdditionalAttributesToRenderTree(builder, sequence: 1);
        AddEventsToRenderTree(builder, sequence: 2);

        builder.OpenElement(7, "span");
        builder.AddAttribute(8, "class", "jfa-anim-transition-layer");
        builder.AddAttribute(9, "style", first);
        builder.AddContent(10, ChildContent);
        builder.CloseElement();

        builder.OpenElement(11, "span");
        builder.AddAttribute(12, "class", "jfa-anim-transition-layer");
        builder.AddAttribute(13, "style", second);
        builder.AddContent(14, AlternateContent);
        builder.CloseElement();

        builder.CloseElement();
    }
}
