//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// An icon that animates between two glyphs whenever <see cref="ShowAlternate"/> changes
/// (for example play/pause or mic on/off). A thin, icon-shaped convenience over
/// <see cref="IconTransition"/>: for arbitrary content use that component directly, and for
/// clickable toggles wrap this in an <see cref="IconButton"/>.
/// </summary>
public class TwoStateIcon : Element {
    private IconStyle? _iconStyleFromConsumer;
    private Typeface? _typefaceFromConsumer;

    /// <summary>
    /// The first (default) state's glyph.
    /// </summary>
    [Parameter]
    public string? IconName { get; set; }

    /// <summary>
    /// The first state's icon style (consumer value wins, then ambient defaults, then solid).
    /// </summary>
    [Parameter]
    public IconStyle IconStyle { get; set; } = IconStyle.Solid;

    /// <summary>
    /// The first state's typeface (consumer value wins, then ambient defaults, then classic).
    /// </summary>
    [Parameter]
    public Typeface Typeface { get; set; } = Typeface.Classic;

    /// <summary>
    /// The first state's glyph color. <c>null</c> inherits the surrounding text color.
    /// </summary>
    [Parameter]
    public string? IconColor { get; set; }

    /// <summary>
    /// The alternate state's glyph.
    /// </summary>
    [Parameter]
    public string? AlternateIconName { get; set; }

    /// <summary>
    /// The alternate state's icon style. Defaults to the resolved first-state style.
    /// </summary>
    [Parameter]
    public IconStyle? AlternateIconStyle { get; set; }

    /// <summary>
    /// The alternate state's glyph color. <c>null</c> inherits the surrounding text color.
    /// </summary>
    [Parameter]
    public string? AlternateColor { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the icon's alternate state is shown.
    /// </summary>
    [Parameter]
    public bool ShowAlternate { get; set; }

    /// <summary>
    /// The transition played when the icon state changes.
    /// </summary>
    [Parameter]
    public IconTransitionKind Transition { get; set; } = IconTransitionKind.Crossfade;

    /// <summary>
    /// The duration of the icon transition. <c>null</c> uses the stylesheet default (0.25s).
    /// </summary>
    [Parameter]
    public TimeSpan? Duration { get; set; }

    private IconStyle ResolvedIconStyle => _iconStyleFromConsumer ?? ResolvedDefaults?.IconStyle ?? IconStyle;

    private Typeface ResolvedTypeface => _typefaceFromConsumer ?? ResolvedDefaults?.Typeface ?? Typeface;

    private IconStyle ResolvedAlternateStyle => AlternateIconStyle ?? ResolvedIconStyle;

    private protected override void CaptureParameters(ParameterView parameters) {
        base.CaptureParameters(parameters);

        _iconStyleFromConsumer = parameters.TryGetValue<IconStyle>(nameof(IconStyle), out var style) ? style : null;
        _typefaceFromConsumer = parameters.TryGetValue<Typeface>(nameof(Typeface), out var typeface) ? typeface : null;
    }

    /// <summary>
    /// Builds the component's render tree: an <see cref="IconTransition"/> with the two glyphs.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenComponent<IconTransition>(0);
        builder.AddAttribute(1, nameof(IconTransition.ShowAlternate), ShowAlternate);
        builder.AddAttribute(2, nameof(IconTransition.Transition), Transition);

        if(Duration.HasValue) {
            builder.AddAttribute(3, nameof(IconTransition.Duration), Duration.Value);
        }

        builder.AddAttribute(4, nameof(IconTransition.ChildContent), (RenderFragment)BuildFirstIcon);
        builder.AddAttribute(5, nameof(IconTransition.AlternateContent), (RenderFragment)BuildSecondIcon);
        builder.CloseComponent();
    }

    private void BuildFirstIcon(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenComponent<Icon>(0);
        builder.AddAttribute(1, nameof(Icon.IconName), IconName);
        builder.AddAttribute(2, nameof(Icon.IconStyle), ResolvedIconStyle);
        builder.AddAttribute(3, nameof(Icon.Typeface), ResolvedTypeface);
        builder.AddAttribute(4, nameof(Icon.Decorative), true);

        if(!string.IsNullOrEmpty(IconColor)) {
            builder.AddAttribute(5, nameof(Icon.Color), IconColor);
        }

        builder.CloseComponent();
    }

    private void BuildSecondIcon(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenComponent<Icon>(0);
        builder.AddAttribute(1, nameof(Icon.IconName), AlternateIconName);
        builder.AddAttribute(2, nameof(Icon.IconStyle), ResolvedAlternateStyle);
        builder.AddAttribute(3, nameof(Icon.Typeface), ResolvedTypeface);
        builder.AddAttribute(4, nameof(Icon.Decorative), true);

        if(!string.IsNullOrEmpty(AlternateColor)) {
            builder.AddAttribute(5, nameof(Icon.Color), AlternateColor);
        }

        builder.CloseComponent();
    }
}
