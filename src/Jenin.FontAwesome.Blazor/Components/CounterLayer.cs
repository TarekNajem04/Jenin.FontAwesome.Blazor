//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// Counter text rendered inside a Font Awesome layers container (<c>fa-layers-counter</c>).
/// </summary>
public class CounterLayer : ElementWithTransformations {
    /// <summary>
    /// The text (typically a number) rendered by the layered counter element. Required: a counter
    /// without text produces no meaningful output.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string? Text { get; set; }

    /// <summary>
    /// Set the counter line height (emitted as <c>--fa-counter-line-height</c>).
    /// </summary>
    [Parameter]
    public string? LineHeight { get; set; }

    /// <summary>
    /// Set the maximum width of the counter (emitted as <c>--fa-counter-max-width</c>).
    /// </summary>
    [Parameter]
    public string? MaxWidth { get; set; }

    /// <summary>
    /// Set the minimum width of the counter (emitted as <c>--fa-counter-min-width</c>).
    /// </summary>
    [Parameter]
    public string? MinWidth { get; set; }

    /// <summary>
    /// Set the counter scale (emitted as <c>--fa-counter-scale</c>).
    /// </summary>
    [Parameter]
    public string? Scale { get; set; }

    /// <summary>
    /// Set the counter's background color (emitted as <c>--fa-counter-background-color</c>).
    /// </summary>
    [Parameter]
    public string? CounterBackgroundColor { get; set; }

    /// <summary>
    /// Set the counter's border radius (emitted as <c>--fa-counter-border-radius</c>).
    /// </summary>
    [Parameter]
    public string? CounterBorderRadius { get; set; }

    /// <summary>
    /// Set the counter's padding (emitted as <c>--fa-counter-padding</c>).
    /// </summary>
    [Parameter]
    public string? CounterPadding { get; set; }

    /// <summary>
    /// Set top offset.
    /// </summary>
    [Parameter]
    public string? Top { get; set; }

    /// <summary>
    /// Set right offset.
    /// </summary>
    [Parameter]
    public string? Right { get; set; }

    /// <summary>
    /// Set bottom offset.
    /// </summary>
    [Parameter]
    public string? Bottom { get; set; }

    /// <summary>
    /// Set left offset.
    /// </summary>
    [Parameter]
    public string? Left { get; set; }

    private protected override List<string?> AddClasses(List<string?> classes) {
        classes = base.AddClasses(classes);
        classes.Add("fa-layers-counter");

        return classes;
    }

    /// <summary>
    /// Builds the component's render tree: the counter element, its attributes, and the counter text.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);
        builder.OpenElement(0, "span");
        AddAdditionalAttributesToRenderTree(builder, sequence: 1);

        if(!string.IsNullOrEmpty(Text)) {
            builder.AddContent(2, Text.Trim());
        }

        builder.CloseElement();
    }

    private protected override Dictionary<string, string> AddStyles(Dictionary<string, string> styles)
        => base.AddStyles(styles)
               .AddOrUpdateIfNotNull("--fa-counter-background-color", CounterBackgroundColor)
               .AddOrUpdateIfNotNull("--fa-counter-border-radius", CounterBorderRadius)
               .AddOrUpdateIfNotNull("--fa-counter-padding", CounterPadding)
               .AddOrUpdateIfNotNull("--fa-counter-line-height", LineHeight)
               .AddOrUpdateIfNotNull("--fa-counter-max-width", MaxWidth)
               .AddOrUpdateIfNotNull("--fa-counter-min-width", MinWidth)
               .AddOrUpdateIfNotNull("--fa-counter-scale", Scale)
               .AddOrUpdateIfNotNull("--fa-top", Top)
               .AddOrUpdateIfNotNull("--fa-right", Right)
               .AddOrUpdateIfNotNull("--fa-bottom", Bottom)
               .AddOrUpdateIfNotNull("--fa-left", Left);
}
