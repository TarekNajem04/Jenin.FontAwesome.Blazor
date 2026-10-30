//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// Overlays a count badge on any child content (typically an <see cref="IconButton"/>).
/// The badge plays a one-shot beat when <see cref="Count"/> changes and exposes
/// <c>role="status"</c> so assistive technology announces updates.
/// </summary>
/// <remarks>
/// <para>
/// The beat uses Font Awesome's <c>fa-beat</c> keyframes, which Font Awesome itself disables
/// under <c>prefers-reduced-motion</c>. Positioning is inline and logical
/// (<c>inset-inline-end</c>), so no stylesheet is required and right-to-left layouts work.
/// </para>
/// </remarks>
public class NotificationBadge : Element {
    private int _lastCount;
    private int _pulseKey;

    /// <summary>
    /// The wrapped content the badge annotates.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// The count displayed on the badge. Values above <see cref="MaxCount"/> render capped
    /// (for example <c>99+</c>).
    /// </summary>
    [Parameter]
    public int Count { get; set; }

    /// <summary>
    /// The largest count rendered exactly; larger values render as <c>{MaxCount}+</c>.
    /// </summary>
    [Parameter]
    public int MaxCount { get; set; } = 99;

    /// <summary>
    /// Gets or sets a value indicating whether a zero count still renders an empty badge.
    /// </summary>
    [Parameter]
    public bool ShowZero { get; set; }

    /// <summary>
    /// The badge background color.
    /// </summary>
    [Parameter]
    public string? BadgeColor { get; set; } = "#dc2626";

    /// <summary>
    /// The badge text color.
    /// </summary>
    [Parameter]
    public string? BadgeTextColor { get; set; } = "#ffffff";

    /// <summary>
    /// Unit noun appended to the announced count (for example <c>unread messages</c>), producing
    /// an accessible name like <c>3 unread messages</c>. Without a unit the badge is announced by
    /// its number alone.
    /// </summary>
    [Parameter]
    public string? Unit { get; set; }

    private string Display => Count > MaxCount ? $"{MaxCount}+" : Convert.ToString(Count, CultureInfo.InvariantCulture);

    private bool HasBadge => ShowZero || Count > 0;

    /// <summary>
    /// Applies parameter changes before rendering: restarts the one-shot beat whenever
    /// <see cref="Count"/> changes by re-keying the badge element.
    /// </summary>
    protected override void OnParametersSet() {
        base.OnParametersSet();

        if(Count != _lastCount) {
            _lastCount = Count;
            _pulseKey++;
        }
    }

    private protected override Dictionary<string, string> AddStyles(Dictionary<string, string> styles) {
        styles = base.AddStyles(styles);

        styles["position"] = "relative";
        styles["display"] = "inline-flex";

        return styles;
    }

    /// <summary>
    /// Builds the component's render tree: the wrapped content with an absolutely positioned
    /// count badge.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenElement(0, "span");
        AddAdditionalAttributesToRenderTree(builder, sequence: 1);
        AddEventsToRenderTree(builder, sequence: 2);
        builder.AddContent(7, ChildContent);

        if(HasBadge) {
            builder.OpenElement(8, "span");
            builder.SetKey(_pulseKey);
            builder.AddAttribute(9, "role", "status");
            builder.AddAttribute(10, "class", "fa-beat");

            var style = new StringBuilder()
                .Append("--fa-animation-iteration-count:1;")
                .Append("position:absolute;")
                .Append("inset-block-start:-0.35em;")
                .Append("inset-inline-end:-0.35em;")
                .Append("display:inline-flex;")
                .Append("align-items:center;")
                .Append("justify-content:center;")
                .Append("min-width:1.25em;")
                .Append("height:1.25em;")
                .Append("padding:0 0.3em;")
                .Append("border-radius:9999px;")
                .Append("font-size:0.65em;")
                .Append("font-weight:700;")
                .Append("line-height:1;")
                .Append(CultureInfo.InvariantCulture, $"background-color:{BadgeColor ?? "#dc2626"};")
                .Append(CultureInfo.InvariantCulture, $"color:{BadgeTextColor ?? "#ffffff"};");

            builder.AddAttribute(11, "style", style.ToString());

            if(!string.IsNullOrWhiteSpace(Unit)) {
                builder.AddAttribute(12, "aria-label", $"{Display} {Unit.Trim()}");
            }

            builder.AddContent(13, Display);
            builder.CloseElement();
        }

        builder.CloseElement();
    }
}
