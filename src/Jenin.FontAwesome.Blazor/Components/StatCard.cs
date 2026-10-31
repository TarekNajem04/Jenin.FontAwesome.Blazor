//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// A metric card: an icon disc, a value, a label, and an optional directional delta.
/// Icon style and typeface follow the consumer value, then ambient
/// <see cref="FontAwesomeDefaults"/>, then the component default — the same precedence
/// <see cref="Icon"/> itself applies. Supply <see cref="IconFragment"/> to render fully custom
/// glyph content instead of the built-in icon.
/// </summary>
public class StatCard : ComposedIconElement {
    /// <summary>
    /// The well-known <c>style</c> attribute name.
    /// </summary>
    private const string STYLE_ATTRIBUTE = "style";

    /// <summary>
    /// The icon shown in the disc. Defaults to a chart glyph.
    /// </summary>
    [Parameter]
    public string? IconName { get; set; } = "fa-chart-line";

    /// <summary>
    /// Set icon style for the stat card (consumer value wins, then ambient defaults, then solid).
    /// </summary>
    [Parameter]
    public IconStyle IconStyle { get; set; } = IconStyle.Solid;

    /// <summary>
    /// Set icon typeface for the stat card (consumer value wins, then ambient defaults, then classic).
    /// </summary>
    [Parameter]
    public Typeface Typeface { get; set; } = Typeface.Classic;

    /// <summary>
    /// The color of the icon shown in the disc. <c>null</c> inherits the surrounding text color.
    /// </summary>
    [Parameter]
    public string? IconColor { get; set; }

    /// <summary>
    /// The icon disc background. <c>null</c> derives a translucent wash from the current text
    /// color, which adapts to light and dark themes automatically.
    /// </summary>
    [Parameter]
    public string? IconBackground { get; set; }

    /// <summary>
    /// Custom glyph content replacing the built-in icon (for duotone, animated, or stacked
    /// markers). When <c>null</c> the built-in icon is rendered.
    /// </summary>
    [Parameter]
    public RenderFragment? IconFragment { get; set; }

    /// <summary>
    /// The metric value (for example <c>1,234</c>).
    /// </summary>
    [Parameter]
    public string? Value { get; set; }

    /// <summary>
    /// The metric label (for example <c>Active users</c>).
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// The delta direction indicator. <see cref="StatDelta.None"/> renders nothing.
    /// </summary>
    [Parameter]
    public StatDelta Delta { get; set; } = StatDelta.None;

    /// <summary>
    /// The delta text (for example <c>+12%</c>).
    /// </summary>
    [Parameter]
    public string? DeltaText { get; set; }

    /// <summary>
    /// The color used for an upward delta.
    /// </summary>
    [Parameter]
    public string? DeltaUpColor { get; set; } = "#16a34a";

    /// <summary>
    /// The color used for a downward delta.
    /// </summary>
    [Parameter]
    public string? DeltaDownColor { get; set; } = "#dc2626";

    private string DeltaGlyph => Delta switch {
        StatDelta.Up => "fa-arrow-trend-up",
        StatDelta.Down => "fa-arrow-trend-down",
        StatDelta.Flat => "fa-minus",
        _ => string.Empty,
    };

    private string? DeltaColor => Delta switch {
        StatDelta.Up => DeltaUpColor,
        StatDelta.Down => DeltaDownColor,
        _ => null,
    };

    private protected override void BuildAccessibilityAttributes() {
        AdditionalAttributes ??= [];

        _ = AdditionalAttributes.AddOrUpdate("role", "group");

        var name = AriaLabel;

        if(string.IsNullOrWhiteSpace(name) && (!string.IsNullOrWhiteSpace(Label) || !string.IsNullOrWhiteSpace(Value))) {
            name = $"{Label}: {Value}".Trim(' ', ':');
        }

        if(!string.IsNullOrWhiteSpace(name)) {
            _ = AdditionalAttributes.AddOrUpdate("aria-label", name.Trim());
        }

        if(!string.IsNullOrWhiteSpace(Title)) {
            _ = AdditionalAttributes.AddOrUpdate("title", Title.Trim());
        }
    }

    /// <summary>
    /// Adds the stat-card layout styles after the shared styles.
    /// </summary>
    /// <param name="styles">The style dictionary to update.</param>
    protected override void AddComponentStyles(Dictionary<string, string> styles) {
        ArgumentNullException.ThrowIfNull(styles);
        styles["display"] = "inline-flex";
        styles["align-items"] = "center";
        styles["gap"] = "0.9rem";
    }

    /// <summary>
    /// Builds the component's render tree: an icon disc beside the value, label, and delta.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        AddRootAndMarker(builder, $"display:inline-flex;align-items:center;justify-content:center;width:2.75rem;height:2.75rem;border-radius:50%;font-size:1.25rem;background-color:{IconBackground ?? "color-mix(in srgb, currentColor 12%, transparent)"};", IconName, IconStyle, Typeface, IconColor, IconFragment);

        builder.OpenElement(16, "span");
        builder.AddAttribute(17, STYLE_ATTRIBUTE, "display:inline-flex;flex-direction:column;line-height:1.25;");

        builder.OpenElement(18, "span");
        builder.AddAttribute(19, STYLE_ATTRIBUTE, "font-size:1.5rem;font-weight:700;");
        builder.AddContent(20, Value);
        builder.CloseElement();

        if(!string.IsNullOrEmpty(Label)) {
            builder.OpenElement(21, "span");
            builder.AddAttribute(22, STYLE_ATTRIBUTE, "font-size:0.85rem;opacity:0.75;");
            builder.AddContent(23, Label);
            builder.CloseElement();
        }

        if(Delta != StatDelta.None && (!string.IsNullOrEmpty(DeltaText) || !string.IsNullOrEmpty(DeltaGlyph))) {
            builder.OpenElement(24, "span");
            builder.AddAttribute(25, STYLE_ATTRIBUTE, $"display:inline-flex;align-items:center;gap:0.3em;font-size:0.8rem;font-weight:600;color:{DeltaColor ?? "inherit"};");

            if(!string.IsNullOrEmpty(DeltaGlyph)) {
                builder.OpenComponent<Icon>(26);
                builder.AddAttribute(27, nameof(Icon.IconName), DeltaGlyph);
                builder.AddAttribute(28, nameof(Icon.IconStyle), IconStyle.Solid);
                builder.AddAttribute(29, nameof(Icon.Decorative), true);
                builder.CloseComponent();
            }

            builder.AddContent(30, DeltaText);
            builder.CloseElement();
        }

        builder.CloseElement();
        builder.CloseElement();
    }
}
