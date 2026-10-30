//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// Text rendered inside a Font Awesome layers container (<c>fa-layers-text</c>).
/// </summary>
public class TextLayer : ElementWithTransformations {
    /// <summary>
    /// The text rendered by the layered text element. Required: a text layer without text
    /// produces no meaningful output.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string? Text { get; set; }

    private protected override List<string?> AddClasses(List<string?> classes) {
        classes = base.AddClasses(classes);
        classes.Add("fa-layers-text");

        return classes;
    }

    private protected override string BuildSignature() => $"{base.BuildSignature()}|text={Text}";

    /// <summary>
    /// Builds the component's render tree: a keyed boundary element wrapping the enhanceable
    /// text element and its text content.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        UpdateBoundaryVersion();

        // Blazor-owned boundary: carries no FA class and is keyed by the FA-visible signature version.
        builder.OpenElement(0, "span");
        builder.SetKey(BoundaryVersion);

        // Enhanceable element: carries the FA attribute contract (fa-layers-text).
        builder.OpenElement(1, "span");
        AddAdditionalAttributesToRenderTree(builder, sequence: 2);

        if(!string.IsNullOrEmpty(Text)) {
            builder.AddContent(3, Text.Trim());
        }

        builder.CloseElement();

        builder.CloseElement();
    }
}
