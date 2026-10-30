//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// Base container for icons; renders its <see cref="Icons"/> and cascades the container to
/// descendant <see cref="Icon"/> components. Supports cascading and events.
/// </summary>
public class IconContainer : Element {
    /// <summary>
    /// The container's child content (typically icons).
    /// </summary>
    /// <remarks>
    /// Not annotated with <c>[EditorRequired]</c>: an empty container is structurally valid.
    /// </remarks>
    [Parameter]
    public RenderFragment? Icons { get; set; }

    private protected virtual string GetRootTag() => "div";

    /// <summary>
    /// Builds the component's render tree: the container root element with its attributes and
    /// events, and the cascading <c>IconContainer</c> value wrapping <see cref="Icons"/>.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenElement(0, GetRootTag());
        AddAdditionalAttributesToRenderTree(builder, sequence: 1);
        AddEventsToRenderTree(builder, sequence: 2);

        CreateCascadingValue
            (
                builder,
                this,
                sequence: 13,
                (builder2) => builder2.AddContent(6, Icons),
                name: "IconContainer",
                isFixed: true
            );
        builder.CloseElement();
    }
}
