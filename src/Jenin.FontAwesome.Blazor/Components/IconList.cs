//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// A Font Awesome icon list (<c>fa-ul</c>).
/// </summary>
/// <remarks>
/// Decorative structure: the inherited event parameters (<c>OnClick</c>, <c>OnDoubleClick</c>,
/// <c>OnMouseOver</c>, <c>OnMouseOut</c>) are not wired by this component. Provide interactivity on
/// the items' content instead.
/// </remarks>
public class IconList : Element {
    /// <summary>
    /// The list items.
    /// </summary>
    /// <remarks>
    /// Not annotated with <c>[EditorRequired]</c>: an empty list is structurally valid.
    /// </remarks>
    [Parameter]
    public RenderFragment? Items { get; set; }

    /// <summary>
    /// Whether the list is rendered as an ordered list (<c>ol</c>) instead of the default
    /// unordered list (<c>ul</c>).
    /// </summary>
    [Parameter]
    public bool OrderedList { get; set; }

    /// <summary>
    /// Set the margin around each list item's icon.
    /// </summary>
    [Parameter]
    public string? ListItemMargin { get; set; }

    /// <summary>
    /// Set the width of each list item's icon.
    /// </summary>
    [Parameter]
    public string? ListItemWidth { get; set; }

    private protected override List<string?> AddClasses(List<string?> classes) {
        classes = base.AddClasses(classes);
        classes.Add("fa-ul");

        return classes;
    }

    private protected override Dictionary<string, string> AddStyles(Dictionary<string, string> styles)
        => base.AddStyles(styles)
               .AddOrUpdateIfNotNull("--fa-li-margin", ListItemMargin)
               .AddOrUpdateIfNotNull("--fa-li-width", ListItemWidth);

    /// <summary>
    /// Builds the component's render tree: the list root element, its attributes, and the items.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenElement(0, OrderedList ? "ol" : "ul");
        AddAdditionalAttributesToRenderTree(builder, sequence: 1);
        builder.AddContent(2, Items);
        builder.CloseElement();
    }
}
