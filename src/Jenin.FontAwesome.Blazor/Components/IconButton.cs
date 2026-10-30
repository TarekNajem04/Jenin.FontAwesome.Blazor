//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// An accessible button that pairs a Font Awesome glyph with an optional visible label.
/// Icon-only buttons must supply <see cref="ElementBase.Title"/> or
/// <see cref="ElementBase.AriaLabel"/> so assistive technology announces an action.
/// </summary>
/// <remarks>
/// <para>
/// Presentation parameters (<see cref="ElementBase.IconSize"/>, <see cref="ElementBase.Color"/>,
/// <see cref="ElementBase.BackgroundColor"/>) apply to the <c>&lt;button&gt;</c> itself and inherit
/// into the glyph; identity parameters (<see cref="IconName"/>, <see cref="IconStyle"/>,
/// <see cref="Typeface"/>, <see cref="Animation"/>) flow to the inner <see cref="Icon"/>, which
/// stays decorative so the button exposes exactly one accessible name.
/// </para>
/// <para>
/// Setting <see cref="Busy"/> swaps the glyph for a spinner without moving the label, disables
/// the button, and sets <c>aria-busy</c>.
/// </para>
/// </remarks>
public class IconButton : Element {
    private IconStyle? _iconStyleFromConsumer;
    private Typeface? _typefaceFromConsumer;

    /// <summary>
    /// The Font Awesome icon name, including its style-independent <c>fa-</c> prefixed identifier
    /// (for example <c>fa-house</c>). When <see cref="Busy"/> is set, <see cref="BusyIconName"/>
    /// is rendered instead.
    /// </summary>
    [Parameter]
    public string? IconName { get; set; }

    /// <summary>
    /// Set icon style. The consumer-supplied value wins, then ambient
    /// <see cref="FontAwesomeDefaults"/>, then this component's own value — the same precedence
    /// <see cref="Icon"/> itself applies.
    /// </summary>
    [Parameter]
    public IconStyle IconStyle { get; set; }

    /// <summary>
    /// Set icon typeface, with the same consumer → ambient-defaults → own-value precedence as
    /// <see cref="Icon"/>.
    /// </summary>
    [Parameter]
    public Typeface Typeface { get; set; }

    /// <summary>
    /// The Font Awesome animation applied to the glyph.
    /// </summary>
    [Parameter]
    public Animation? Animation { get; set; }

    /// <summary>
    /// Visible text rendered after the glyph. It is also the accessible name, so no
    /// <c>aria-label</c> is emitted when a label is present.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the button shows its busy state: the glyph is
    /// replaced by <see cref="BusyIconName"/> with <see cref="BusyAnimation"/>, the button is
    /// disabled, and <c>aria-busy</c> is set. The label is preserved so no layout shift occurs.
    /// </summary>
    [Parameter]
    public bool Busy { get; set; }

    /// <summary>
    /// The glyph rendered while <see cref="Busy"/> is set.
    /// </summary>
    [Parameter]
    public string? BusyIconName { get; set; } = "fa-spinner";

    /// <summary>
    /// The icon style used for <see cref="BusyIconName"/>.
    /// </summary>
    [Parameter]
    public IconStyle BusyIconStyle { get; set; } = IconStyle.Solid;

    /// <summary>
    /// The animation applied to <see cref="BusyIconName"/>.
    /// </summary>
    [Parameter]
    public Animation? BusyAnimation { get; set; } = Animate.Spin();

    /// <summary>
    /// Gets or sets a value indicating whether the button is disabled.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    private protected virtual string? CurrentIconName => Busy ? BusyIconName : IconName;

    /// <summary>
    /// The style forwarded to the glyph: the busy style, else the consumer value, else ambient
    /// defaults, else this component's own value.
    /// </summary>
    private protected virtual IconStyle ForwardIconStyle
        => Busy ? BusyIconStyle : _iconStyleFromConsumer ?? ResolvedDefaults?.IconStyle ?? IconStyle;

    /// <summary>
    /// The typeface forwarded to the glyph, resolved with the same precedence as the style.
    /// </summary>
    private protected virtual Typeface ForwardTypeface
        => _typefaceFromConsumer ?? ResolvedDefaults?.Typeface ?? Typeface;

    private protected virtual Animation? CurrentAnimation => Busy ? BusyAnimation : Animation;

    private protected virtual string? CurrentLabel => Label;

    private protected virtual string? CurrentTitle => Title;

    /// <summary>
    /// The explicit accessible name: an author-supplied <c>aria-label</c>, else the tooltip for
    /// icon-only buttons. A visible label already names the button, so no <c>aria-label</c> is
    /// emitted for labeled buttons.
    /// </summary>
    private protected virtual string? CurrentAriaLabel => AriaLabel ?? (Label is null ? Title : null);

    private protected virtual string? CurrentIconColor => null;

    /// <summary>
    /// Applies parameter changes before rendering: maintains the <c>aria-busy</c> attribute
    /// contract for the persistent additional-attributes dictionary.
    /// </summary>
    protected override void OnParametersSet() {
        base.OnParametersSet();

        AdditionalAttributes ??= [];

        if(Busy) {
            _ = AdditionalAttributes.AddOrUpdate("aria-busy", "true");
        } else {
            _ = AdditionalAttributes.Remove("aria-busy");
        }
    }

    private protected override void CaptureParameters(ParameterView parameters) {
        base.CaptureParameters(parameters);

        _iconStyleFromConsumer = parameters.TryGetValue<IconStyle>(nameof(IconStyle), out var iconStyle) ? iconStyle : null;
        _typefaceFromConsumer = parameters.TryGetValue<Typeface>(nameof(Typeface), out var typeface) ? typeface : null;
    }

    private protected override void BuildAccessibilityAttributes() {
        AdditionalAttributes ??= [];

        var name = CurrentAriaLabel;

        if(!string.IsNullOrWhiteSpace(name)) {
            _ = AdditionalAttributes.AddOrUpdate("aria-label", name.Trim());
        }

        var title = CurrentTitle;

        if(!string.IsNullOrWhiteSpace(title)) {
            _ = AdditionalAttributes.AddOrUpdate("title", title.Trim());
        }

        if(IsDecorative) {
            _ = AdditionalAttributes.AddOrUpdate("aria-hidden", "true");
        }
    }

    /// <summary>
    /// Builds the component's render tree: a native <c>&lt;button&gt;</c> hosting a decorative
    /// <see cref="Icon"/> and the optional visible label.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenElement(0, "button");
        builder.AddAttribute(1, "type", "button");
        builder.AddAttribute(2, "disabled", Disabled || Busy);
        AddAdditionalAttributesToRenderTree(builder, sequence: 3);
        AddEventsToRenderTree(builder, sequence: 4);

        builder.OpenComponent<Icon>(10);
        builder.AddAttribute(11, nameof(Icon.IconName), CurrentIconName);
        builder.AddAttribute(12, nameof(Icon.IconStyle), ForwardIconStyle);
        builder.AddAttribute(13, nameof(Icon.Typeface), ForwardTypeface);

        if(CurrentAnimation is not null) {
            builder.AddAttribute(14, nameof(Icon.Animation), CurrentAnimation);
        }

        if(CurrentIconColor is not null) {
            builder.AddAttribute(15, nameof(Icon.Color), CurrentIconColor);
        }

        builder.AddAttribute(16, nameof(Icon.Decorative), true);
        builder.CloseComponent();

        var label = CurrentLabel;

        if(!string.IsNullOrEmpty(label)) {
            builder.AddContent(17, " ");
            builder.AddContent(18, label);
        }

        builder.CloseElement();
    }
}
