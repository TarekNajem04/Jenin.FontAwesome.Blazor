//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// A circular avatar showing initials or a glyph, with an optional presence dot.
/// </summary>
/// <remarks>
/// <para>
/// Supply <see cref="Initials"/> for text avatars, or child content (typically an
/// <see cref="Icon"/>) for glyph avatars; with neither, a default user glyph renders.
/// The presence dot is a <see cref="StatusDot"/> positioned over the avatar edge.
/// </para>
/// </remarks>
public class AvatarIcon : Element {
    /// <summary>
    /// The content rendered when <see cref="Initials"/> is not supplied (typically an
    /// <see cref="Icon"/>, which inherits the avatar text color and scales with it).
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Initials rendered as avatar text. Takes precedence over <see cref="ChildContent"/>.
    /// </summary>
    [Parameter]
    public string? Initials { get; set; }

    /// <summary>
    /// The avatar diameter in CSS pixels.
    /// </summary>
    [Parameter]
    public int Size { get; set; } = 40;

    /// <summary>
    /// The avatar background color.
    /// </summary>
    [Parameter]
    public string? AvatarColor { get; set; } = "#6c757d";

    /// <summary>
    /// The ring color separating the presence dot from the avatar.
    /// </summary>
    [Parameter]
    public string? RingColor { get; set; } = "#ffffff";

    /// <summary>
    /// The presence state. <see cref="AvatarPresence.None"/> renders no dot.
    /// </summary>
    [Parameter]
    public AvatarPresence Presence { get; set; } = AvatarPresence.None;

    private static string PresenceColor(AvatarPresence presence) => presence switch {
        AvatarPresence.Online => "#22c55e",
        AvatarPresence.Busy => "#ef4444",
        AvatarPresence.Away => "#f59e0b",
        AvatarPresence.Offline => "#9ca3af",
        _ => "#9ca3af",
    };

    private protected override Dictionary<string, string> AddStyles(Dictionary<string, string> styles) {
        styles = base.AddStyles(styles);

        styles["position"] = "relative";
        styles["display"] = "inline-flex";
        styles["align-items"] = "center";
        styles["justify-content"] = "center";
        styles["border-radius"] = "50%";
        styles["overflow"] = "hidden";
        styles["font-weight"] = "600";

        return styles.AddOrUpdateIfNotNull("width", $"{Size.ToString(CultureInfo.InvariantCulture)}px")
                     .AddOrUpdateIfNotNull("height", $"{Size.ToString(CultureInfo.InvariantCulture)}px")
                     .AddOrUpdateIfNotNull("background-color", AvatarColor)
                     .AddOrUpdateIfNotNull("font-size", $"{(Size * 0.4).ToString("F2", CultureInfo.InvariantCulture)}px");
    }

    private protected override void BuildAccessibilityAttributes() {
        AdditionalAttributes ??= [];

        AccessibilityAttributes.ApplyNameRole(AdditionalAttributes, "img", AriaLabel ?? Title ?? Initials, IsDecorative, Title);
    }

    /// <summary>
    /// Builds the component's render tree: the avatar circle with initials or glyph content and
    /// an optional presence dot.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenElement(0, "span");
        AddAdditionalAttributesToRenderTree(builder, sequence: 1);
        AddEventsToRenderTree(builder, sequence: 2);

        if(!string.IsNullOrEmpty(Initials)) {
            builder.AddContent(7, Initials);
        } else if(ChildContent is not null) {
            builder.AddContent(7, ChildContent);
        } else {
            builder.OpenComponent<Icon>(7);
            builder.AddAttribute(8, nameof(Icon.IconName), "fa-user");
            builder.AddAttribute(9, nameof(Icon.IconStyle), IconStyle.Solid);
            builder.AddAttribute(10, nameof(Icon.Decorative), true);
            builder.CloseComponent();
        }

        if(Presence != AvatarPresence.None) {
            var diameter = $"{(Size * 0.32).ToString("F2", CultureInfo.InvariantCulture)}px";

            builder.OpenComponent<StatusDot>(11);
            builder.AddAttribute(12, nameof(StatusDot.BackgroundColor), PresenceColor(Presence));
            builder.AddAttribute(13, nameof(StatusDot.Diameter), diameter);
            builder.AddAttribute(14, nameof(StatusDot.Decorative), true);
            builder.AddAttribute(15, "style", $"position:absolute;inset-block-end:0;inset-inline-end:0;box-shadow:0 0 0 2px {RingColor ?? "#ffffff"};");
            builder.CloseComponent();
        }

        builder.CloseElement();
    }
}
