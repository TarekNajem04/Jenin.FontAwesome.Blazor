//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// A timeline/activity row: an icon marker beside a title, body content, and an optional timestamp.
/// Icon style and typeface follow the consumer value, then ambient
/// <see cref="FontAwesomeDefaults"/>, then the component default. Supply
/// <see cref="IconFragment"/> to render fully custom marker content instead of the built-in icon.
/// </summary>
public class ActivityItem : ComposedIconElement {
    /// <summary>
    /// The marker glyph. Defaults to a bell.
    /// </summary>
    [Parameter]
    public string? IconName { get; set; } = "fa-bell";

    /// <summary>
    /// Set icon style for the activity item (consumer value wins, then ambient defaults, then solid).
    /// </summary>
    [Parameter]
    public IconStyle IconStyle { get; set; } = IconStyle.Solid;

    /// <summary>
    /// Set icon typeface for the activity item (consumer value wins, then ambient defaults, then classic).
    /// </summary>
    [Parameter]
    public Typeface Typeface { get; set; } = Typeface.Classic;

    /// <summary>
    /// The marker glyph color. <c>null</c> inherits the surrounding text color.
    /// </summary>
    [Parameter]
    public string? IconColor { get; set; }

    /// <summary>
    /// The marker disc background. <c>null</c> derives a translucent wash from the current text
    /// color, which adapts to light and dark themes automatically.
    /// </summary>
    [Parameter]
    public string? MarkerBackground { get; set; }

    /// <summary>
    /// Custom marker content replacing the built-in icon.
    /// </summary>
    [Parameter]
    public RenderFragment? IconFragment { get; set; }

    /// <summary>
    /// The timestamp rendered beneath the title. The title itself comes from
    /// <see cref="ElementBase.Title"/> and also serves as the tooltip.
    /// </summary>
    [Parameter]
    public string? Time { get; set; }

    /// <summary>
    /// The item body rendered beneath the title.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Adds the item layout styles after the shared styles.
    /// </summary>
    /// <param name="styles">The style dictionary to update.</param>
    protected override void AddComponentStyles(Dictionary<string, string> styles) {
        ArgumentNullException.ThrowIfNull(styles);
        styles["display"] = "flex";
        styles["gap"] = "0.75rem";
        styles["align-items"] = "flex-start";
    }

    /// <summary>
    /// Builds the component's render tree: the marker disc beside the title, body, and time.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        AddRootAndMarker(builder, $"display:inline-flex;align-items:center;justify-content:center;flex:none;width:2rem;height:2rem;border-radius:50%;font-size:0.9rem;background-color:{MarkerBackground ?? "color-mix(in srgb, currentColor 12%, transparent)"};", IconName, IconStyle, Typeface, IconColor, IconFragment);

        builder.OpenElement(16, "span");
        builder.AddAttribute(17, "style", "display:inline-flex;flex-direction:column;gap:0.15rem;min-width:0;");

        if(!string.IsNullOrEmpty(Title)) {
            builder.OpenElement(18, "span");
            builder.AddAttribute(19, "style", "font-weight:600;");
            builder.AddContent(20, Title);
            builder.CloseElement();
        }

        if(ChildContent is not null) {
            builder.AddContent(21, ChildContent);
        }

        if(!string.IsNullOrEmpty(Time)) {
            builder.OpenElement(22, "span");
            builder.AddAttribute(23, "style", "font-size:0.8rem;opacity:0.65;");
            builder.AddContent(24, Time);
            builder.CloseElement();
        }

        builder.CloseElement();
        builder.CloseElement();
    }
}
