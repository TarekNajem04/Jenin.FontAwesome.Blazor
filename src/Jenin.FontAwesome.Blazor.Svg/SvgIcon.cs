//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Svg;

/// <summary>
/// Base class for generated self-contained Font Awesome SVG icons. Each icon renders a single
/// inline <c>&lt;svg&gt;</c> with its own path data — no Font Awesome JavaScript, CSS, or web
/// fonts are involved, and unreferenced icons are linker-trimmed like any other unused type.
/// </summary>
/// <remarks>
/// Icons are decorative by default; supply <see cref="Title"/> or <see cref="AriaLabel"/> to
/// expose an accessible name. Color inherits from surrounding text (<c>currentColor</c>);
/// override per instance with <see cref="CssClass"/>.
/// </remarks>
public abstract class SvgIcon : ComponentBase {
    /// <summary>
    /// The SVG path data for the icon, normalized to a <c>0 0 512 512</c> viewBox.
    /// </summary>
    protected abstract string PathData { get; }

    /// <summary>
    /// An optional CSS class applied to the <c>&lt;svg&gt;</c> element.
    /// </summary>
    [Parameter]
    public string? CssClass { get; set; }

    /// <summary>
    /// Accessible name and tooltip for the icon. Emitted as a <c>&lt;title&gt;</c> child and an
    /// <c>aria-label</c>; supplying a name marks the icon as meaningful rather than decorative.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Accessible name emitted as the <c>aria-label</c> attribute. Supplying a name marks the
    /// icon as meaningful rather than decorative.
    /// </summary>
    [Parameter]
    public string? AriaLabel { get; set; }

    /// <summary>
    /// Captures all unmatched values so consumers can pass through arbitrary attributes
    /// (for example <c>width</c>, <c>height</c>, or event handlers) alongside the built-ins.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Builds the component's render tree: a sized inline <c>&lt;svg&gt;</c> with the icon's
    /// path data.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        string? accessibleName = string.IsNullOrWhiteSpace(AriaLabel) ? null : AriaLabel.Trim();
        accessibleName ??= string.IsNullOrWhiteSpace(Title) ? null : Title.Trim();

        builder.OpenElement(0, "svg");
        builder.AddAttribute(1, "xmlns", "http://www.w3.org/2000/svg");
        builder.AddAttribute(2, "viewBox", "0 0 512 512");

        var userWidth = GetUserAttribute("width");
        var userHeight = GetUserAttribute("height");

        // A consumer width/height wins; otherwise the default 1em height keeps the glyph on the
        // text line (width derives from the square viewBox).
        builder.AddAttribute(3, "height", string.IsNullOrWhiteSpace(userHeight) ? "1em" : userHeight.Trim());

        if(!string.IsNullOrWhiteSpace(userWidth)) {
            builder.AddAttribute(4, "width", userWidth.Trim());
        }

        var mergedClass = string.Join(
            ' ',
            new[] { CssClass, GetUserAttribute("class") }.Where(static part => !string.IsNullOrWhiteSpace(part)));

        if(!string.IsNullOrWhiteSpace(mergedClass)) {
            builder.AddAttribute(5, "class", mergedClass);
        }

        builder.AddAttribute(6, "style", "vertical-align:-0.125em;" + (GetUserAttribute("style") ?? string.Empty));

        if(accessibleName is null) {
            builder.AddAttribute(7, "aria-hidden", "true");
        } else {
            builder.AddAttribute(7, "role", "img");
            builder.AddAttribute(8, "aria-label", accessibleName);
        }

        AppendRestAttributes(builder);

        if(accessibleName is not null && !string.IsNullOrWhiteSpace(Title)) {
            builder.OpenElement(10, "title");
            builder.AddContent(11, Title.Trim());
            builder.CloseElement();
        }

        builder.OpenElement(12, "path");
        builder.AddAttribute(13, "d", PathData);
        builder.AddAttribute(14, "fill", "currentColor");
        builder.CloseElement();

        builder.CloseElement();
    }

    private string? GetUserAttribute(string name) =>
        AdditionalAttributes is not null && AdditionalAttributes.TryGetValue(name, out var value)
            ? Convert.ToString(value, CultureInfo.InvariantCulture)
            : null;

    private void AppendRestAttributes(RenderTreeBuilder builder) {
        if(AdditionalAttributes is null) {
            return;
        }

        Dictionary<string, object>? rest = null;

        foreach(var attribute in AdditionalAttributes) {
            if(attribute.Key is "class" or "style" or "xmlns" or "viewBox" or "height" or "width"
                or "aria-hidden" or "role" or "aria-label" or "title" || attribute.Value is null) {
                continue;
            }

            rest ??= new Dictionary<string, object>(StringComparer.Ordinal);
            rest[attribute.Key] = attribute.Value;
        }

        if(rest is not null) {
            builder.AddMultipleAttributes(9, rest);
        }
    }
}
