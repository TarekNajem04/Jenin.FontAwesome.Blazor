//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// Renders the circular/plain marker wrapper shared by the composed icon components. Internal:
/// it is an implementation detail, not public API.
/// </summary>
internal sealed class IconMarker : ComponentBase {
    [Parameter]
    public string? MarkerStyle { get; set; }

    [Parameter]
    public string? IconName { get; set; }

    [Parameter]
    public IconStyle IconStyle { get; set; }

    [Parameter]
    public Typeface Typeface { get; set; }

    [Parameter]
    public string? IconColor { get; set; }

    [Parameter]
    public RenderFragment? IconFragment { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(0, "span");
        builder.AddAttribute(1, "aria-hidden", "true");
        builder.AddAttribute(2, "style", MarkerStyle);
        IconForwarding.AddGlyph(builder, 3, IconName, IconStyle, Typeface, IconColor, IconFragment);
        builder.CloseElement();
    }
}
