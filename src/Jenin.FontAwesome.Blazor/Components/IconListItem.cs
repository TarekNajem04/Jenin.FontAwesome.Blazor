//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// A single Font Awesome icon list item (<c>fa-li</c>).
/// </summary>
/// <remarks>
/// Decorative structure: the inherited event parameters are only wired to the item's marker
/// element, not the item as a whole. Provide interactivity on the item content instead.
/// </remarks>
public class IconListItem : Element {
    /// <summary>
    /// The icon marker rendered by the item.
    /// </summary>
    /// <remarks>
    /// Not annotated with <c>[EditorRequired]</c>: a list item without a marker is structurally valid.
    /// </remarks>
    [Parameter]
    public RenderFragment? ItemIcon { get; set; }

    /// <summary>
    /// The content rendered next to the item's icon marker.
    /// </summary>
    /// <remarks>
    /// Not annotated with <c>[EditorRequired]</c>: empty item content is structurally valid.
    /// </remarks>
    [Parameter]
    public RenderFragment? ItemContent { get; set; }

    /// <summary>
    /// Set the margin around the list item's icon.
    /// </summary>
    [Parameter]
    public string? ItemMargin { get; set; }

    /// <summary>
    /// Set the width of the list item's icon.
    /// </summary>
    [Parameter]
    public string? ItemWidth { get; set; }

    private protected override List<string?> AddClasses(List<string?> classes) {
        classes = base.AddClasses(classes);
        classes.Add("fa-li");

        return classes;
    }

    private protected override Dictionary<string, string> AddStyles(Dictionary<string, string> styles)
        => base.AddStyles(styles)
               .AddOrUpdateIfNotNull("--fa-li-margin", ItemMargin)
               .AddOrUpdateIfNotNull("--fa-li-width", ItemWidth);

    /// <summary>
    /// Builds the component's render tree: the list item, the icon marker span, and the item content.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenElement(0, "li");
        builder.OpenElement(1, "span");

        AddAdditionalAttributesToRenderTree(builder, sequence: 2);
        AddEventsToRenderTree(builder, sequence: 3);

        builder.AddContent(14, ItemIcon);
        builder.CloseElement();
        builder.AddContent(15, ItemContent);
        builder.CloseElement();
    }
}
