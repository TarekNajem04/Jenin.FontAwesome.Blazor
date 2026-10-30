//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// A progress ring with centered child content (typically an <see cref="Icon"/>), exposed as a
/// <c>role="progressbar"</c> with its value. Self-contained: the ring is inline SVG, so no
/// stylesheet or JavaScript is required.
/// </summary>
public class IconProgress : Element {
    /// <summary>
    /// The content centered inside the ring.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// The progress fraction in the inclusive range 0 to 1. Values outside the range are clamped.
    /// </summary>
    [Parameter]
    public float Progress { get; set; }

    /// <summary>
    /// The ring diameter in CSS pixels.
    /// </summary>
    [Parameter]
    public int Size { get; set; } = 48;

    /// <summary>
    /// The ring stroke width in CSS pixels.
    /// </summary>
    [Parameter]
    public int StrokeWidth { get; set; } = 4;

    /// <summary>
    /// The track (background) ring color.
    /// </summary>
    [Parameter]
    public string? TrackColor { get; set; } = "#e5e7eb";

    /// <summary>
    /// The progress (foreground) ring color.
    /// </summary>
    [Parameter]
    public string? BarColor { get; set; } = "#0d6efd";

    private float Clamped => MathF.Min(1, MathF.Max(0, Progress));

    private int Percent => (int)MathF.Round(Clamped * 100);

    private protected override Dictionary<string, string> AddStyles(Dictionary<string, string> styles) {
        styles = base.AddStyles(styles);

        styles["position"] = "relative";
        styles["display"] = "inline-flex";
        styles["width"] = $"{Size.ToString(CultureInfo.InvariantCulture)}px";
        styles["height"] = $"{Size.ToString(CultureInfo.InvariantCulture)}px";

        return styles;
    }

    private protected override void BuildAccessibilityAttributes() {
        AdditionalAttributes ??= [];

        _ = AdditionalAttributes.AddOrUpdate("role", "progressbar");
        _ = AdditionalAttributes.AddOrUpdate("aria-valuemin", "0");
        _ = AdditionalAttributes.AddOrUpdate("aria-valuemax", "100");
        _ = AdditionalAttributes.AddOrUpdate("aria-valuenow", Percent.ToString(CultureInfo.InvariantCulture));

        var name = AriaLabel ?? Title;

        if(!string.IsNullOrWhiteSpace(name)) {
            _ = AdditionalAttributes.AddOrUpdate("aria-label", name.Trim());
        }

        if(!string.IsNullOrWhiteSpace(Title)) {
            _ = AdditionalAttributes.AddOrUpdate("title", Title.Trim());
        }
    }

    /// <summary>
    /// Builds the component's render tree: an inline-SVG ring with the child content centered over it.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        var size = Math.Max(1, Size);
        var stroke = Math.Max(1, Math.Min(StrokeWidth, size / 2));
        var radius = (size - stroke) / 2.0;
        var circumference = 2 * Math.PI * radius;
        var offset = circumference * (1 - Clamped);
        var center = size / 2.0;

        static string Format(double value) => value.ToString("F2", CultureInfo.InvariantCulture);

        builder.OpenElement(0, "span");
        AddAdditionalAttributesToRenderTree(builder, sequence: 1);
        AddEventsToRenderTree(builder, sequence: 2);

        builder.OpenElement(7, "svg");
        builder.AddAttribute(8, "width", size.ToString(CultureInfo.InvariantCulture));
        builder.AddAttribute(9, "height", size.ToString(CultureInfo.InvariantCulture));
        builder.AddAttribute(10, "viewBox", $"0 0 {size.ToString(CultureInfo.InvariantCulture)} {size.ToString(CultureInfo.InvariantCulture)}");
        builder.AddAttribute(11, "aria-hidden", "true");

        builder.OpenElement(12, "circle");
        builder.AddAttribute(13, "cx", Format(center));
        builder.AddAttribute(14, "cy", Format(center));
        builder.AddAttribute(15, "r", Format(radius));
        builder.AddAttribute(16, "fill", "none");
        builder.AddAttribute(17, "stroke", TrackColor ?? "#e5e7eb");
        builder.AddAttribute(18, "stroke-width", stroke.ToString(CultureInfo.InvariantCulture));
        builder.CloseElement();

        builder.OpenElement(19, "circle");
        builder.AddAttribute(20, "cx", Format(center));
        builder.AddAttribute(21, "cy", Format(center));
        builder.AddAttribute(22, "r", Format(radius));
        builder.AddAttribute(23, "fill", "none");
        builder.AddAttribute(24, "stroke", BarColor ?? "#0d6efd");
        builder.AddAttribute(25, "stroke-width", stroke.ToString(CultureInfo.InvariantCulture));
        builder.AddAttribute(26, "stroke-linecap", "round");
        builder.AddAttribute(27, "stroke-dasharray", Format(circumference));
        builder.AddAttribute(28, "stroke-dashoffset", Format(offset));
        builder.AddAttribute(29, "transform", $"rotate(-90 {Format(center)} {Format(center)})");
        builder.CloseElement();

        builder.CloseElement();

        builder.OpenElement(30, "span");
        builder.AddAttribute(31, "style", $"position:absolute;inset:0;display:flex;align-items:center;justify-content:center;font-size:{Format(size * 0.4)}px;");
        builder.AddAttribute(32, "aria-hidden", "true");
        builder.AddContent(33, ChildContent);
        builder.CloseElement();

        builder.CloseElement();
    }
}
