//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// Base type for Font Awesome element components: owns the shared style, accessibility, and
/// additional-attribute building pipeline. Not typically consumed directly.
/// </summary>
public class ElementBase : ComponentBase {
    /// <summary>
    /// The well-known <c>style</c> attribute name.
    /// </summary>
    private const string StyleAttribute = "style";

    #region Properties
    /// <summary>
    /// Custom css classname.
    /// </summary>
    [Parameter]
    public string? CssClass { get; set; }

    /// <summary>
    /// sets the background color of an element
    /// </summary>
    [Parameter]
    public string? BackgroundColor { get; set; }

    /// <summary>
    /// sets the text color of an element
    /// </summary>
    [Parameter]
    public string? Color { get; set; }

    /// <summary>
    /// Set icon size.
    /// </summary>
    [Parameter]
    public IconSize IconSize { get; set; }

    /// <summary>
    /// Make all your icons the same width so they can easily vertically align, like in a list or navigation menu.
    /// <para>
    /// Changes an icon's font-size to 0.75em
    /// </para>
    /// </summary>
    /// <remarks>
    /// Font Awesome 7 renders icons at a fixed width by default and deprecates the emitted
    /// <c>fa-fw</c> class. Prefer <see cref="WidthMode"/> for the Font Awesome 7 width classes.
    /// </remarks>
    [Parameter]
    public bool FixedWidth { get; set; }

    /// <summary>
    /// Controls the Font Awesome width behavior: <see cref="IconWidth.Auto"/> opts out of the
    /// default fixed Icon Canvas width (<c>fa-width-auto</c>) and <see cref="IconWidth.Fixed"/>
    /// re-applies the legacy fixed width (<c>fa-width-fixed</c>).
    /// <see cref="IconWidth.NotSet"/> emits nothing.
    /// </summary>
    [Parameter]
    public IconWidth WidthMode { get; set; }

    /// <summary>
    /// Applies the Font Awesome Icon Canvas padding utility: <see cref="CanvasPadding.Square"/>
    /// (<c>fa-canvas-square</c>) or <see cref="CanvasPadding.Roomy"/> (<c>fa-canvas-roomy</c>).
    /// <see cref="CanvasPadding.NotSet"/> emits nothing.
    /// </summary>
    [Parameter]
    public CanvasPadding CanvasPadding { get; set; }
    /// <summary>
    /// Creates a border around an icons
    /// </summary>
    /// <remarks>
    /// If you set this property to <c>true</c>, all border properties will be applied.
    /// <para>
    /// <see cref="BorderColor"/>, <see cref="BorderPadding"/> <see cref="BorderRadius"/>, <see cref="BorderStyle"/>, <see cref="BorderWidth"/>
    /// </para>
    /// </remarks>
    [Parameter]
    public bool Bordered { get; set; }

    /// <summary>
    /// Set border color.
    /// </summary>
    [Parameter]
    public string? BorderColor { get; set; }

    /// <summary>
    /// Set padding around.
    /// </summary>
    [Parameter]
    public string? BorderPadding { get; set; }

    /// <summary>
    /// Set border radius.
    /// </summary>
    [Parameter]
    public string? BorderRadius { get; set; }

    /// <summary>
    /// Set border style.
    /// </summary>
    [Parameter]
    public BorderStyle BorderStyle { get; set; }

    /// <summary>
    /// Set border width.
    /// </summary>
    [Parameter]
    public string? BorderWidth { get; set; }

    /// <summary>
    /// Padding is used to create space around an element's content, inside of any defined borders.
    /// <para>
    /// top, right, bottom, left
    /// </para>
    /// </summary>
    [Parameter]
    public string? Padding { get; set; }

    /// <summary>
    /// Margins are used to create space around elements, outside of any defined borders.
    /// <para>
    /// top, right, bottom, left
    /// </para>
    /// </summary>
    [Parameter]
    public string? Margin { get; set; }

    /// <summary>
    /// Inverts the color of an icon to white.
    /// </summary>
    [Parameter]
    public bool Inverse { get; set; }

    /// <summary>
    /// Set color of an inverted icon.
    /// </summary>
    [Parameter]
    public string? InverseColor { get; set; }

    /// <summary>
    /// Accessible name and tooltip for the element. Emitted as the Font Awesome <c>title</c>
    /// attribute, which Font Awesome moves into an SVG <c>&lt;title&gt;</c> element during
    /// enhancement. Supplying a value marks the element as meaningful rather than decorative.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Accessible name emitted as the <c>aria-label</c> attribute. Supplying a value marks the
    /// element as meaningful rather than decorative.
    /// </summary>
    [Parameter]
    public string? AriaLabel { get; set; }

    /// <summary>
    /// Whether the element is decorative (hidden from assistive technology).
    /// <para>
    /// <c>null</c> (the default) infers the mode: the element is decorative when it has no
    /// accessible name (<see cref="Title"/> or <see cref="AriaLabel"/>) and the component is
    /// decorative by default. <c>true</c> forces decorative; <c>false</c> forces meaningful.
    /// </para>
    /// </summary>
    [Parameter]
    public bool? Decorative { get; set; }

    /// <summary>
    /// Captures all unmatched values passed to the component so they are emitted on the
    /// underlying element alongside the built class, style, and accessibility attributes.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
    #endregion Properties

    private string? _lastSignature;
    private IconSize? _iconSizeFromParent;
    private FontAwesomeOptions? _serviceDefaults;
    private bool _serviceDefaultsResolved;
    private string? _userStyle;
    private string? _userClass;
    private bool _userAttributesCaptured;

    /// <summary>
    /// Ambient defaults supplied by an enclosing <see cref="FontAwesomeDefaults"/> component.
    /// </summary>
    [CascadingParameter]
    private protected FontAwesomeOptions? Defaults { get; set; }

    /// <summary>
    /// Service provider used to resolve options registered with <c>AddJeninFontAwesome</c>. Injected
    /// lazily because the options registration is optional.
    /// </summary>
    [Inject]
    private protected IServiceProvider? Services { get; set; }

    /// <summary>
    /// Monotonic version of the FA-visible signature. It is used as the render key of the
    /// Blazor-owned boundary element so that a signature change recreates the boundary subtree
    /// and Font Awesome re-enhances the fresh subtree.
    /// </summary>
    private protected int BoundaryVersion { get; private set; }

    /// <summary>
    /// Effective library-wide defaults: registered options overlaid by the nearest
    /// <see cref="FontAwesomeDefaults"/> cascade. <c>null</c> when nothing is configured.
    /// </summary>
    private protected FontAwesomeOptions? ResolvedDefaults { get; private set; }

    /// <summary>
    /// Records the parameters supplied by the parent, then defers to the base component to run
    /// the standard parameter lifecycle.
    /// </summary>
    /// <param name="parameters">The parameters supplied by the parent for this render.</param>
    /// <returns>A task that completes when the parameters have been applied.</returns>
    public override Task SetParametersAsync(ParameterView parameters) {
        _iconSizeFromParent = parameters.TryGetValue<IconSize>(nameof(IconSize), out var iconSize) ? iconSize : null;

        CaptureParameters(parameters);

        return base.SetParametersAsync(parameters);
    }

    /// <summary>
    /// Captures the author-supplied <c>style</c> and <c>class</c> values the first time parameters are
    /// set, before this component writes its own computed values into <see cref="AdditionalAttributes"/>.
    /// The captured values are the base the computed styles and classes are layered onto on every
    /// subsequent render, so a computed value that disappears (for example a cleared colour) is not
    /// re-emitted from the value written on a previous render.
    /// </summary>
    private void CaptureUserStyleAndClass() {
        if(_userAttributesCaptured) {
            return;
        }

        _userAttributesCaptured = true;

        if(AdditionalAttributes is null) {
            return;
        }

        if(AdditionalAttributes.TryGetValue(StyleAttribute, out var style)) {
            _userStyle = Convert.ToString(style, CultureInfo.InvariantCulture);
        }

        if(AdditionalAttributes.TryGetValue("class", out var @class)) {
            _userClass = Convert.ToString(@class, CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// Records which component-specific parameters the parent supplied during this render. Derived
    /// components override this to capture their own parameters so that omitted parameters fall back
    /// to configured defaults.
    /// </summary>
    /// <param name="parameters">The parameters supplied by the parent for this render.</param>
    private protected virtual void CaptureParameters(ParameterView parameters) {
    }

    /// <summary>
    /// Applies parameter changes before rendering: resolves the effective defaults and rebuilds
    /// the additional attributes.
    /// </summary>
    protected override void OnParametersSet() {
        base.OnParametersSet();

        ResolvedDefaults = FontAwesomeOptions.Merge(GetServiceDefaults(), Defaults);

        CaptureUserStyleAndClass();
        BuildAdditionalAttributes();
    }

    private FontAwesomeOptions? GetServiceDefaults() {
        if(!_serviceDefaultsResolved) {
            _serviceDefaults = Services?.GetService(typeof(FontAwesomeOptions)) as FontAwesomeOptions;
            _serviceDefaultsResolved = true;
        }

        return _serviceDefaults;
    }

    /// <summary>
    /// Gets a value indicating whether the parent supplied the <see cref="IconSize"/> parameter during
    /// this render, as opposed to either omitting it or receiving a configured default.
    /// </summary>
    private protected bool HasIconSizeFromParent => _iconSizeFromParent.HasValue;

    /// <summary>
    /// Gets the effective icon size: the explicit parameter, else the configured default, else the
    /// component's own <see cref="IconSize"/> value (which a derived preset may bake in), else
    /// <see cref="IconSize.NotSet"/>.
    /// </summary>
    private protected IconSize EffectiveIconSize => _iconSizeFromParent ?? ResolvedDefaults?.IconSize ?? IconSize;

    /// <summary>
    /// Gets a value indicating whether this component is hidden from assistive technology when it
    /// has no accessible name. Icon-like leaf elements default to decorative; containers do not.
    /// </summary>
    private protected virtual bool IsDecorativeByDefault => false;

    /// <summary>
    /// Gets a value indicating whether an accessible name (<see cref="Title"/> or
    /// <see cref="AriaLabel"/>) has been supplied.
    /// </summary>
    private protected bool HasAccessibleName =>
        !string.IsNullOrWhiteSpace(Title) || !string.IsNullOrWhiteSpace(AriaLabel);

    /// <summary>
    /// Gets the effective decorative state. An explicit <see cref="Decorative"/> wins, then a configured
    /// default; otherwise the element is decorative when it has no accessible name and
    /// <see cref="IsDecorativeByDefault"/>.
    /// </summary>
    private protected bool IsDecorative =>
        Decorative ?? ResolvedDefaults?.Decorative ?? (!HasAccessibleName && IsDecorativeByDefault);

    private protected virtual void BuildAdditionalAttributes() {
        AdditionalAttributes ??= [];

        // Restore the author-supplied base values before recomputing, so computed values written on a
        // previous render never leak into the next one (BuildStyles reseeds from this dictionary).
        _ = AdditionalAttributes.SetOrRemoveIfEmpty("class", _userClass);
        _ = AdditionalAttributes.SetOrRemoveIfEmpty(StyleAttribute, _userStyle);

        _ = AdditionalAttributes.SetOrRemoveIfEmpty("class", BuildClasses().Trim());
        _ = AdditionalAttributes.SetOrRemoveIfEmpty(StyleAttribute, BuildStyles().Trim());

        BuildAccessibilityAttributes();
    }

    /// <summary>
    /// Emits the accessible-name (<c>title</c>, <c>aria-label</c>) and decorative (<c>aria-hidden</c>)
    /// attributes into <see cref="AdditionalAttributes"/>. Derived components extend this to add their
    /// role semantics.
    /// </summary>
    private protected virtual void BuildAccessibilityAttributes() {
        AdditionalAttributes ??= [];

        if(!string.IsNullOrWhiteSpace(Title)) {
            _ = AdditionalAttributes.AddOrUpdate("title", Title.Trim());
        }

        if(!string.IsNullOrWhiteSpace(AriaLabel)) {
            _ = AdditionalAttributes.AddOrUpdate("aria-label", AriaLabel.Trim());
        }

        if(IsDecorative) {
            _ = AdditionalAttributes.AddOrUpdate("aria-hidden", "true");
        }
    }

    /// <summary>
    /// Recomputes the FA-visible signature and increments <see cref="BoundaryVersion"/> when it changed.
    /// </summary>
    private protected void UpdateBoundaryVersion() {
        var signature = BuildSignature();

        if(!string.Equals(signature, _lastSignature, StringComparison.Ordinal)) {
            _lastSignature = signature;
            BoundaryVersion++;
        }
    }

    /// <summary>
    /// Deterministic serialization of the FA-visible attribute contract. Derived components append
    /// their consumed content and structural markers.
    /// </summary>
    private protected virtual string BuildSignature() {
        if(AdditionalAttributes is null || AdditionalAttributes.Count == 0) {
            return string.Empty;
        }

        var signature = new StringBuilder();

        foreach(var attribute in AdditionalAttributes.OrderBy(static x => x.Key, StringComparer.Ordinal)) {
            _ = signature.Append(attribute.Key)
                         .Append('=')
                         .Append(Convert.ToString(attribute.Value, CultureInfo.InvariantCulture))
                         .Append(';');
        }

        return signature.ToString();
    }

    private protected string BuildClasses() {
        var classes = new List<string?>() { CssClass };

        if(FixedWidth) {
            classes.Add("fa-fw");
        }

        var iconSizeCss = EffectiveIconSize.GetStringValue();

        if(!string.IsNullOrEmpty(iconSizeCss)) {
            classes.Add(iconSizeCss);
        }

        if(Bordered) {
            classes.Add("fa-border");
        }

        if(Inverse) {
            classes.Add("fa-inverse");
        }

        if(WidthMode == IconWidth.Auto) {
            classes.Add("fa-width-auto");
        } else if(WidthMode == IconWidth.Fixed) {
            classes.Add("fa-width-fixed");
        }

        var canvasPaddingCss = CanvasPadding.GetStringValue();

        if(!string.IsNullOrEmpty(canvasPaddingCss)) {
            classes.Add(canvasPaddingCss);
        }

        classes = AddClasses(classes);

        return string.Join(' ', classes.Where(x => !string.IsNullOrEmpty(x))).Trim();
    }
    private protected virtual List<string?> AddClasses(List<string?> classes) => classes;

    private protected string BuildStyles() {
        var styles = new Dictionary<string, string>();

        // ===== Check exist Style =====
        if(AdditionalAttributes?.TryGetValue(StyleAttribute, out var existStyles) == true) {
            var existStylesString = Convert.ToString(existStyles, CultureInfo.InvariantCulture);

            if(!string.IsNullOrEmpty(existStylesString)) {
                foreach(var style in existStylesString.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)) {
                    var kvp = style.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    if(kvp.Length == 2) {
                        styles.Add(kvp[0], kvp[1]);
                    }
                }
            }
        }

        styles = styles.AddOrUpdateIfNotNull("--fa-inverse", InverseColor);

        if(Bordered) {
            styles = styles.AddOrUpdateIfNotNull("--fa-border-color", BorderColor)
                           .AddOrUpdateIfNotNull("--fa-border-padding", BorderPadding)
                           .AddOrUpdateIfNotNull("--fa-border-radius", BorderRadius)
                           .AddOrUpdateIf("--fa-border-style", BorderStyle, value => value is not BorderStyle.None)
                           .AddOrUpdateIfNotNull("--fa-border-width", BorderWidth);
        }

        styles = styles.AddOrUpdateIfNotNull("background-color", BackgroundColor)
                       .AddOrUpdateIfNotNull("color", Color)
                       .AddOrUpdateIfNotNull("padding", Padding)
                       .AddOrUpdateIfNotNull("margin", Margin);

        styles = AddStyles(styles);

        return styles.JoinAsStyles();
    }

    /// <summary>
    /// Add a component that provides a cascading value to all descendant components <see cref="CascadingValue{TValue}"/>.
    /// <para>
    /// &lt;CascadingValue Value=this Name="CascadingValue name" IsFixed=true|false&gt;@ChildContent&lt;/CascadingValue&gt;
    /// </para>
    /// </summary>
    /// <typeparam name="TValue">The type of the cascading value</typeparam>
    /// <param name="builder">The render tree builder</param>
    /// <param name="value">The value to provide</param>
    /// <param name="sequence">The sequence number</param>
    /// <param name="childContent">The child content</param>
    /// <param name="name">The name of the cascading value</param>
    /// <param name="isFixed">A value indicating whether the cascading value is fixed</param>
    /// <remarks>The sequence is incremented by 4 [sequence  + 4]</remarks>
    private protected static void CreateCascadingValue<TValue>
        (
            RenderTreeBuilder builder,
            TValue value,
            int sequence,
            RenderFragment childContent,
            string name,
            bool isFixed
        ) {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenRegion(sequence);
        builder.OpenComponent<CascadingValue<TValue>>(sequence);
        builder.AddAttribute(0, "Value", value);
        builder.AddAttribute(1, "Name", name);
        builder.AddAttribute(2, "IsFixed", isFixed);
        builder.AddAttribute(3, "ChildContent", childContent);
        builder.CloseComponent();
        builder.CloseRegion();
    }

    private protected virtual Dictionary<string, string> AddStyles(Dictionary<string, string> styles) => styles;

    private protected virtual void AddAdditionalAttributesToRenderTree(RenderTreeBuilder builder) => AddAdditionalAttributesToRenderTree(builder, 100);
    private protected virtual void AddAdditionalAttributesToRenderTree(RenderTreeBuilder builder, int sequence) {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddMultipleAttributes(sequence, AdditionalAttributes);
    }
}
