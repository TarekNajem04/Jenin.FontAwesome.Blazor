//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Blazorise;

/// <summary>
/// A typed Font Awesome icon for Blazorise trees: renders Blazorise's <c>Icon</c> with the
/// mapped name, style, and size, so suite users get FA 7 through the component they already
/// use. Requires a Blazorise icon provider at runtime (for example
/// <c>Blazorise.Icons.FontAwesome</c> with <c>AddFontAwesomeIcons()</c>).
/// </summary>
/// <remarks>
/// Only name, style, size, and fixed-width map cleanly; suite-foreign features (animations,
/// transforms, duotone layering) stay native-only — compose them with this library's own
/// <c>Icon</c> component instead. Custom kit glyphs flow through unchanged.
/// </remarks>
public class JeninBlazoriseIcon : ComponentBase {
    /// <summary>
    /// The Font Awesome icon name, with or without the <c>fa-</c> prefix (for example
    /// <c>fa-phone</c>). Custom kit glyphs flow through unchanged.
    /// </summary>
    [Parameter]
    public string? IconName { get; set; }

    /// <summary>
    /// The Font Awesome style, mapped to Blazorise's <c>IconStyle</c>. <c>null</c> (the default),
    /// <c>Thin</c>, and <c>Brands</c> fall back to the provider default.
    /// </summary>
    [Parameter]
    public global::Jenin.FontAwesome.Blazor.IconStyle? IconStyle { get; set; }

    /// <summary>
    /// The Font Awesome size, mapped to Blazorise's <c>IconSize</c>. Unmapped steps fall back
    /// to the provider default; size via CSS instead.
    /// </summary>
    [Parameter]
    public global::Jenin.FontAwesome.Blazor.IconSize? IconSize { get; set; }

    /// <summary>
    /// Renders the icon within a fixed-width slot, forwarded to <c>Icon.FixedWidth</c>.
    /// </summary>
    [Parameter]
    public bool FixedWidth { get; set; }

    /// <summary>
    /// Captures unmatched values and forwards them to the rendered <c>Icon</c>
    /// (for example <c>title</c>, which Blazorise's <c>Icon</c> accepts as an attribute).
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenComponent<global::Blazorise.Icon>(0);
        builder.AddAttribute(1, "Name", BlazoriseIconMapper.ToIconName(IconName));
        builder.AddAttribute(2, "IconStyle", BlazoriseIconMapper.ToBlazoriseStyle(IconStyle));
        builder.AddAttribute(3, "IconSize", BlazoriseIconMapper.ToBlazoriseSize(IconSize));

        if(FixedWidth) {
            builder.AddAttribute(4, "FixedWidth", true);
        }

        if(AdditionalAttributes is not null) {
            var forwarded = AdditionalAttributes
                .Where(static pair => pair.Value is not null)
                .ToDictionary(static pair => pair.Key, static pair => pair.Value!);

            builder.AddMultipleAttributes(5, forwarded);
        }

        builder.CloseComponent();
    }
}
