//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.MudBlazor;

/// <summary>
/// A typed Font Awesome icon for MudBlazor trees: renders MudBlazor's <c>MudIcon</c> with the
/// mapped Font Awesome class string, so suite users get FA 7 through the slot they already
/// use (<c>MudIcon</c>, <c>MudIconButton.Icon</c>, <c>MudNavLink.Icon</c>, and friends accept
/// the same strings — see <see cref="MudIconMapper"/>).
/// </summary>
/// <remarks>
/// Only name, style, title, size, color, and CSS passthrough map cleanly; suite-foreign
/// features (animations, transforms, duotone layering) stay native-only — compose them with
/// this library's own <c>Icon</c> component instead.
/// </remarks>
public class JeninMudIcon : ComponentBase {
    /// <summary>
    /// The Font Awesome icon name, with or without the <c>fa-</c> prefix (for example
    /// <c>fa-house</c>). Custom kit glyphs flow through unchanged.
    /// </summary>
    [Parameter]
    public string? IconName { get; set; }

    /// <summary>
    /// The Font Awesome style used for the class prefix. Defaults to
    /// <see cref="IconStyle.Solid"/>.
    /// </summary>
    [Parameter]
    public IconStyle IconStyle { get; set; } = IconStyle.Solid;

    /// <summary>
    /// Tooltip and accessible name, forwarded to <c>MudIcon.Title</c>.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// MudBlazor icon size, forwarded unchanged. Defaults to <c>Size.Medium</c>.
    /// Use <see cref="MudIconMapper.ToMudSize"/> to convert an <see cref="IconSize"/>.
    /// </summary>
    [Parameter]
    public global::MudBlazor.Size Size { get; set; } = global::MudBlazor.Size.Medium;

    /// <summary>
    /// MudBlazor theme color, forwarded unchanged. Defaults to <c>Color.Inherit</c>.
    /// </summary>
    [Parameter]
    public global::MudBlazor.Color Color { get; set; } = global::MudBlazor.Color.Inherit;

    /// <summary>
    /// Additional CSS classes forwarded to <c>MudIcon.Class</c>.
    /// </summary>
    [Parameter]
    public string? CssClass { get; set; }

    /// <summary>
    /// Captures unmatched values and forwards them to the rendered <c>MudIcon</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenComponent<global::MudBlazor.MudIcon>(0);
        builder.AddAttribute(1, nameof(global::MudBlazor.MudIcon.Icon), MudIconMapper.ToFontClass(IconName, IconStyle));
        builder.AddAttribute(2, nameof(global::MudBlazor.MudIcon.Title), Title);
        builder.AddAttribute(3, nameof(global::MudBlazor.MudIcon.Size), Size);
        builder.AddAttribute(4, nameof(global::MudBlazor.MudIcon.Color), Color);
        builder.AddAttribute(5, nameof(global::MudBlazor.MudIcon.Class), CssClass);

        if(AdditionalAttributes is not null) {
            var forwarded = AdditionalAttributes
                .Where(static pair => pair.Value is not null)
                .ToDictionary(static pair => pair.Key, static pair => pair.Value!);

            builder.AddMultipleAttributes(6, forwarded);
        }

        builder.CloseComponent();
    }
}
