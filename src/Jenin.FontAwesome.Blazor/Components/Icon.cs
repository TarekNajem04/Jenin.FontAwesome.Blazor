//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// Core Font Awesome icon rendering with style, size, rotation, pull, masking, duotone,
/// animations, transformations, and events.
/// </summary>
public class Icon : ElementWithTransformations {
    /// <summary>
    /// The ambient <see cref="IconContainer"/> received from the surrounding container, if any.
    /// </summary>
    /// <remarks>
    /// Exposed for host and preset-consumer extensibility; <see cref="Icon"/> does not consume the
    /// value itself (container-supplied presentation flows through the emitted FA contract).
    /// </remarks>
    [CascadingParameter(Name = "IconContainer")]
    public IconContainer? Container { get; set; }

    /// <summary>
    /// Set icon typeface.
    /// </summary>
    [Parameter]
    public Typeface Typeface { get; set; }

    /// <summary>
    /// Set icon style.
    /// </summary>
    [Parameter]
    public IconStyle IconStyle { get; set; }

    /// <summary>
    /// The Font Awesome icon name, including its style-independent <c>fa-</c> prefixed identifier
    /// (for example <c>fa-house</c> or <c>fa-circle-plus</c>).
    /// </summary>
    /// <remarks>
    /// Intentionally not annotated with <c>[EditorRequired]</c>: preset components derive from
    /// <see cref="Icon"/> and supply <see cref="IconName"/> themselves, so a required marker would
    /// raise a false advisory on every preset. When an icon is authored directly, provide a name.
    /// </remarks>
    [Parameter]
    public string? IconName { get; set; }

    /// <summary>
    /// To rotate, flip, or mirror an icon for it to work in your project or design.
    /// </summary>
    [Parameter]
    public IconRotate Rotate { get; set; }

    /// <summary>
    /// To make rotating icons more customizable and specific
    /// </summary>
    [Parameter]
    public float? RotateAngle { get; set; }

    /// <summary>
    /// You can both rotate and flip an icon.
    /// </summary>
    [Parameter]
    public IconRotate CombiningRotate { get; set; }

    /// <summary>
    /// To create animations like beat, fade, beat-fade, flip, and spin for your icons,
    /// so that you can create more visual interest on your site.
    /// </summary>
    [Parameter]
    public Animation? Animation { get; set; }

    /// <summary>
    /// Controls when <see cref="Animation"/> runs (immediately, on hover, on parent hover, on
    /// focus, or on scroll into view). Hover, parent-hover, and focus triggers pause the
    /// keyframes until the interaction; scroll-reveal hides the icon until it enters the
    /// viewport. Requires the shipped <c>animation-triggers.css</c> stylesheet, and
    /// <c>animation-triggers.js</c> for <see cref="AnimationTrigger.ScrollReveal"/>.
    /// </summary>
    [Parameter]
    public AnimationTrigger Trigger { get; set; } = AnimationTrigger.None;

    /// <summary>
    /// Pulls the icon out of flow to the left or right of surrounding text
    /// (<c>fa-pull-left</c> or <c>fa-pull-right</c>). Combine with <see cref="PullMargin"/>.
    /// </summary>
    [Parameter]
    public PullPosition PullPosition { get; set; }

    /// <summary>
    /// Set the margin around a pulled icon.
    /// </summary>
    [Parameter]
    public string? PullMargin { get; set; }

    /// <summary>
    /// Set stack size.
    /// </summary>
    [Parameter]
    public IconStackSize StackSize { get; set; }

    /// <summary>
    /// Set z-index of a stacked icon
    /// </summary>
    [Parameter]
    public int? StackZIndex { get; set; }

    /// <summary>
    /// Set mask icon style.
    /// <para>
    /// <a href="https://fontawesome.com/docs/web/style/mask">Masking</a>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Combine two icons create one single-color shape.
    /// Use it with <see cref="Transform"/> for some really awesome effects.
    /// </remarks>
    [Parameter]
    public IconStyle MaskIconStyle { get; set; }

    /// <summary>
    /// Set mask icon name.
    /// <para>
    /// <a href="https://fontawesome.com/docs/web/style/mask">Masking</a>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Combine two icons create one single-color shape.
    /// Use it with <see cref="Transform"/> for some really awesome effects.
    /// </remarks>
    [Parameter]
    public string? MaskIconName { get; set; }

    /// <summary>
    /// Set mask icon id, emitted as the <c>data-fa-mask-id</c> attribute so a specific mask is
    /// referenced when several masks are combined.
    /// <para>
    /// <a href="https://fontawesome.com/docs/web/style/mask">Masking</a>
    /// </para>
    /// </summary>
    [Parameter]
    public string? MaskId { get; set; }

    /// <summary>
    /// Set duotone primary layer color
    /// </summary>
    [Parameter]
    public string? PrimaryColor { get; set; }

    /// <summary>
    /// Set duotone primary layer opacity (between 0 and 1)
    /// </summary>
    [Parameter]
    public float? PrimaryOpacity { get; set; }

    /// <summary>
    /// Set duotone secondary layer color
    /// </summary>
    [Parameter]
    public string? SecondaryColor { get; set; }

    /// <summary>
    /// Set duotone secondary layer opacity (between 0 and 1)
    /// </summary>
    [Parameter]
    public float? SecondaryOpacity { get; set; }

    /// <summary>
    /// Swap the default opacity of each layer in a duotone icon
    /// (making an icon's primary layer have the default opacity of 40% rather than its secondary layer)
    /// </summary>
    [Parameter]
    public bool SwapOpacity { get; set; }

    private Typeface? _typefaceFromParent;
    private IconStyle? _iconStyleFromParent;

    private protected override void CaptureParameters(ParameterView parameters) {
        base.CaptureParameters(parameters);

        _typefaceFromParent = parameters.TryGetValue<Typeface>(nameof(Typeface), out var typeface) ? typeface : null;
        _iconStyleFromParent = parameters.TryGetValue<IconStyle>(nameof(IconStyle), out var iconStyle) ? iconStyle : null;
    }

    /// <summary>
    /// Gets the effective typeface: the explicit parameter, else the configured default, else the
    /// component's own <see cref="Typeface"/> value (which a derived preset may bake in), else
    /// <see cref="Typeface.Classic"/>.
    /// </summary>
    private protected Typeface EffectiveTypeface => _typefaceFromParent ?? ResolvedDefaults?.Typeface ?? Typeface;

    /// <summary>
    /// Gets the effective icon style: the explicit parameter, else the configured default, else the
    /// component's own <see cref="IconStyle"/> value (which a derived preset may bake in), else
    /// <see cref="IconStyle.Solid"/>.
    /// </summary>
    private protected IconStyle EffectiveIconStyle => _iconStyleFromParent ?? ResolvedDefaults?.IconStyle ?? IconStyle;

    private protected string? GetMaskIcon() => !string.IsNullOrEmpty(MaskIconName) ? $"{MaskIconStyle.GetStringValue()} {MaskIconName}" : null;
    private protected string? GetMaskId() => !string.IsNullOrEmpty(MaskId) ? MaskId : null;

    /// <summary>
    /// Applies parameter changes before rendering: resolves a registered custom glyph, clamps the
    /// duotone opacities and emits the mask attribute contract.
    /// </summary>
    protected override void OnParametersSet() {
        _customIcon = ResolveCustomIcon();

        PrimaryOpacity = NormalizeOpacity(PrimaryOpacity);
        SecondaryOpacity = NormalizeOpacity(SecondaryOpacity);

        base.OnParametersSet();

        AdditionalAttributes ??= [];

        if(_customIcon is null) {
            _ = AdditionalAttributes.UpdateOrRemoveIf("data-fa-mask", GetMaskIcon(), value => string.IsNullOrEmpty(Convert.ToString(value, CultureInfo.InvariantCulture)));
            _ = AdditionalAttributes.UpdateOrRemoveIf("data-fa-mask-id", GetMaskId(), value => string.IsNullOrEmpty(Convert.ToString(value, CultureInfo.InvariantCulture)));
        }
    }

    private CustomIconDefinition? _customIcon;

    private CustomIconDefinition? ResolveCustomIcon() {
        if(string.IsNullOrWhiteSpace(IconName)) {
            return null;
        }

        if(Services?.GetService(typeof(FontAwesomeOptions)) is not FontAwesomeOptions options
            || options.CustomIcons.Count == 0) {
            return null;
        }

        var name = IconName.Trim();

        if(options.CustomIcons.TryGetValue(name, out var direct)) {
            return direct;
        }

        var alternative = name.StartsWith("fa-", StringComparison.OrdinalIgnoreCase) ? name[3..] : "fa-" + name;
        return options.CustomIcons.TryGetValue(alternative, out var fallback) ? fallback : null;
    }

    /// <summary>
    /// Builds the component's render tree: a keyed boundary element that hosts the events and
    /// either the enhanceable Font Awesome element or, for registered custom glyphs, a
    /// self-contained inline SVG (no FA engine involvement).
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        UpdateBoundaryVersion();

        // Blazor-owned boundary: carries no FA class, hosts events, and is keyed by the FA-visible
        // signature version. When the signature changes the key changes and Blazor recreates the whole
        // subtree, letting Font Awesome (MutationObserver) re-enhance the fresh element.
        builder.OpenElement(0, "span");
        builder.SetKey(BoundaryVersion);
        AddEventsToRenderTree(builder, sequence: 1);

        if(_customIcon is not null) {
            BuildCustomGlyphTree(builder, _customIcon);
            builder.CloseElement();
            return;
        }

        // Enhanceable element: carries the FA attribute contract.
        builder.OpenElement(6, "i");
        AddAdditionalAttributesToRenderTree(builder, sequence: 7);

        builder.CloseElement();
        builder.CloseElement();
    }

    private void BuildCustomGlyphTree(RenderTreeBuilder builder, CustomIconDefinition definition) {
        // Self-contained glyph: mirrors SvgIcon's contract (1em box, currentColor, optional
        // title) so custom icons render identically with or without Font Awesome assets.
        // No fa-* classes are emitted, keeping the FA engine from touching the element.
        builder.OpenElement(6, "svg");
        builder.AddAttribute(7, "xmlns", "http://www.w3.org/2000/svg");
        builder.AddAttribute(8, "viewBox", definition.ViewBox);

        var hasUserHeight = AdditionalAttributes is not null
            && AdditionalAttributes.TryGetValue("height", out var userHeight)
            && !string.IsNullOrWhiteSpace(Convert.ToString(userHeight, CultureInfo.InvariantCulture));
        if(!hasUserHeight) {
            builder.AddAttribute(9, "height", "1em");
        }

        AddAdditionalAttributesToRenderTree(builder, sequence: 10);
        builder.AddAttribute(15, "data-icon", definition.BareName);

        builder.OpenElement(20, "path");
        builder.AddAttribute(21, "d", definition.PathData);
        builder.AddAttribute(22, "fill", "currentColor");
        builder.CloseElement();

        var accessibleName = !string.IsNullOrWhiteSpace(AriaLabel) ? AriaLabel.Trim() : Title?.Trim();
        if(!string.IsNullOrEmpty(accessibleName)) {
            builder.OpenElement(30, "title");
            builder.AddContent(31, accessibleName);
            builder.CloseElement();
        }

        builder.CloseElement();
    }

    /// <summary>
    /// Icons are decorative by default: a bare glyph carries no meaning for assistive technology.
    /// </summary>
    private protected override bool IsDecorativeByDefault => true;

    private protected override void BuildAccessibilityAttributes() {
        base.BuildAccessibilityAttributes();

        AdditionalAttributes ??= [];

        if(IsDecorative) {
            _ = AdditionalAttributes.AddOrUpdate("role", "presentation");
        } else {
            // Meaningful icon: expose it as an image and make sure Font Awesome's SVG enhancement does
            // not hide it. The accessible name comes from Title (title) or AriaLabel (aria-label).
            _ = AdditionalAttributes.AddOrUpdate("role", "img");
            _ = AdditionalAttributes.AddOrUpdate("aria-hidden", "false");
        }
    }

    private protected override List<string?> AddClasses(List<string?> classes) {
        classes = base.AddClasses(classes);

        if(_customIcon is not null) {
            // Registered custom glyphs are self-contained: no FA tokens, so the FA engine
            // (and missing-glyph fallbacks) never touch the element. Generic utilities
            // (CssClass, sizes, borders) still apply when their CSS is present.
            return classes;
        }

        classes.AddRange
                (
                    [
                        EffectiveTypeface.GetStringValue(),
                        EffectiveIconStyle.GetStringValue(),
                        IconName,
                        Rotate.GetStringValue(),
                        Animation?.BuildClasses(),
                        BuildTriggerClass(),
                        BuildContainerClasses(),
                        StackSize.GetStringValue(),
                        SwapOpacity ? "fa-swap-opacity" : null,
                    ]
                );

        return classes;
    }

    private protected override Dictionary<string, string> AddStyles(Dictionary<string, string> styles) {
        styles = base.AddStyles(styles);

        if(Rotate is IconRotate.RotateBy && RotateAngle.HasValue) {
            _ = styles.AddOrUpdate("--fa-rotate-angle", $"{RotateAngle.Value.ToString("F2", CultureInfo.InvariantCulture)}deg");
        }

        if(Animation is not null) {
            foreach(var (key, value) in Animation.BuildStyles()) {
                _ = styles.AddOrUpdate(key, value);
            }
        }

        styles = BuildContainerStyles(styles);

        return styles.AddOrUpdateIfNotNull("--fa-stack-z-index", StackZIndex)
                     .AddOrUpdateIfNotNull("--fa-primary-color", PrimaryColor)
                     .AddOrUpdateIfNotNull("--fa-primary-opacity", PrimaryOpacity)
                     .AddOrUpdateIfNotNull("--fa-secondary-color", SecondaryColor)
                     .AddOrUpdateIfNotNull("--fa-secondary-opacity", SecondaryOpacity);
    }

    private string? BuildTriggerClass() => Trigger switch {
        AnimationTrigger.Hover => "jfa-anim-hover",
        AnimationTrigger.ParentHover => "jfa-anim-parent-hover",
        AnimationTrigger.Focus => "jfa-anim-focus",
        AnimationTrigger.ScrollReveal => "jfa-anim-reveal-target",
        _ => null,
    };

    private protected virtual string BuildContainerClasses()
        => string.Join
                  (
                    ' ',
                    CombiningRotate.GetStringValue(),
                    PullPosition is not PullPosition.None ? PullPosition.GetStringValue() : null
                  )
                 .Trim();

    private protected virtual Dictionary<string, string> BuildContainerStyles(Dictionary<string, string> styles) {
        if(CombiningRotate is IconRotate.RotateBy && RotateAngle.HasValue) {
            _ = styles.AddOrUpdate("--fa-rotate-angle", $"{RotateAngle.Value.ToString("F2", CultureInfo.InvariantCulture)}deg");
        }

        if(PullPosition is not PullPosition.None && !string.IsNullOrEmpty(PullMargin)) {
            _ = styles.AddOrUpdate("--fa-pull-margin", PullMargin);
        }

        return styles;
    }

    private static float? NormalizeOpacity(float? value) => value.HasValue ? MathF.Min(1, MathF.Max(0, value.Value)) : value;
}
