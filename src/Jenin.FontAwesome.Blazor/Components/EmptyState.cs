//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// A centered empty-state block: a large glyph, a title, a description, and an action slot.
/// Icon style and typeface follow the consumer value, then ambient
/// <see cref="FontAwesomeDefaults"/>, then the component default. Supply
/// <see cref="IconFragment"/> to render fully custom glyph content instead of the built-in icon.
/// </summary>
public class EmptyState : ComposedIconElement {
    /// <summary>
    /// The glyph shown above the title. Defaults to an inbox glyph.
    /// </summary>
    [Parameter]
    public string? IconName { get; set; } = "fa-inbox";

    /// <summary>
    /// Set icon style for the empty state (consumer value wins, then ambient defaults, then solid).
    /// </summary>
    [Parameter]
    public IconStyle IconStyle { get; set; } = IconStyle.Solid;

    /// <summary>
    /// Set icon typeface for the empty state (consumer value wins, then ambient defaults, then classic).
    /// </summary>
    [Parameter]
    public Typeface Typeface { get; set; } = Typeface.Classic;

    /// <summary>
    /// The color of the glyph shown above the title. <c>null</c> inherits the surrounding text color.
    /// </summary>
    [Parameter]
    public string? IconColor { get; set; }

    /// <summary>
    /// Custom glyph content replacing the built-in icon.
    /// </summary>
    [Parameter]
    public RenderFragment? IconFragment { get; set; }

    /// <summary>
    /// The description rendered beneath the title.
    /// </summary>
    [Parameter]
    public string? Description { get; set; }

    /// <summary>
    /// The action slot (typically buttons) rendered beneath the description.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Adds the empty-state layout styles after the shared styles.
    /// </summary>
    /// <param name="styles">The style dictionary to update.</param>
    protected override void AddComponentStyles(Dictionary<string, string> styles) {
        ArgumentNullException.ThrowIfNull(styles);
        styles["display"] = "inline-flex";
        styles["flex-direction"] = "column";
        styles["align-items"] = "center";
        styles["gap"] = "0.5rem";
        styles["text-align"] = "center";
        styles["padding"] = "2rem 1rem";
    }

    /// <summary>
    /// Builds the component's render tree: the glyph, title, description, and action slot.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        AddRootAndMarker(builder, "font-size:3rem;opacity:0.6;line-height:1;", IconName, IconStyle, Typeface, IconColor, IconFragment);

        if(!string.IsNullOrEmpty(Title)) {
            builder.OpenElement(16, "div");
            builder.AddAttribute(17, "style", "font-size:1.1rem;font-weight:600;");
            builder.AddContent(18, Title);
            builder.CloseElement();
        }

        if(!string.IsNullOrEmpty(Description)) {
            builder.OpenElement(19, "div");
            builder.AddAttribute(20, "style", "font-size:0.9rem;opacity:0.75;max-width:28rem;");
            builder.AddContent(21, Description);
            builder.CloseElement();
        }

        if(ChildContent is not null) {
            builder.AddContent(22, ChildContent);
        }

        builder.CloseElement();
    }
}
