//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// A small presence dot filled with <see cref="ElementBase.BackgroundColor"/> (green by default).
/// Labeled dots expose <c>role="status"</c> with the supplied name; unlabeled dots are hidden
/// from assistive technology.
/// </summary>
/// <remarks>
/// <para>
/// Setting <see cref="Pulse"/> animates the dot with Font Awesome's <c>fa-beat</c> keyframes,
/// which Font Awesome itself disables under <c>prefers-reduced-motion</c>.
/// </para>
/// </remarks>
public class StatusDot : Element {
    /// <summary>
    /// The dot diameter as any valid CSS size. Relative units scale with the surrounding text.
    /// </summary>
    [Parameter]
    public string? Diameter { get; set; } = "0.625em";

    /// <summary>
    /// Gets or sets a value indicating whether the dot pulses.
    /// </summary>
    [Parameter]
    public bool Pulse { get; set; }

    private protected override List<string?> AddClasses(List<string?> classes) {
        classes = base.AddClasses(classes);

        if(Pulse) {
            classes.Add("fa-beat");
        }

        return classes;
    }

    private protected override Dictionary<string, string> AddStyles(Dictionary<string, string> styles) {
        styles = base.AddStyles(styles);

        styles["display"] = "inline-block";
        styles["border-radius"] = "50%";
        styles["background-color"] = BackgroundColor ?? "#22c55e";

        return styles.AddOrUpdateIfNotNull("width", Diameter)
                     .AddOrUpdateIfNotNull("height", Diameter);
    }

    private protected override void BuildAccessibilityAttributes() {
        AdditionalAttributes ??= [];

        AccessibilityAttributes.ApplyNameRole(AdditionalAttributes, "status", AriaLabel ?? Title, IsDecorative, Title);
    }

    /// <summary>
    /// Builds the component's render tree: a single dot element.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenElement(0, "span");
        AddAdditionalAttributesToRenderTree(builder, sequence: 1);
        AddEventsToRenderTree(builder, sequence: 2);
        builder.CloseElement();
    }
}
