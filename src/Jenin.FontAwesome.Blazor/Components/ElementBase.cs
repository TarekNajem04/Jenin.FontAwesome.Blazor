namespace Jenin.FontAwesome.Blazor.Components;

public class ElementBase : OwningComponentBase {
    #region Properties
    /// <summary>
    /// Sometimes, when changing some properties, it does not rebuild the render tree of the element,
    /// so this property forces the render tree to be rebuild for that element.
    /// <para>
    /// <a href="https://learn.microsoft.com/en-us/aspnet/core/blazor/advanced-scenarios?view=aspnetcore-6.0#sequence-numbers-relate-to-code-line-numbers-and-not-execution-order/">
    /// Sequence numbers relate to code line numbers and not execution order
    /// </a>
    /// </para>
    /// </summary>
    [Parameter]
    public bool ForceRebuildRenderTree { get; set; }

    /// <summary>
    /// Custom css classname.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; }

    /// <summary>
    /// sets the background color of an element
    /// </summary>
    [Parameter]
    public string BackgroundColor { get; set; }

    /// <summary>
    /// sets the text color of an element
    /// </summary>
    [Parameter]
    public string Color { get; set; }

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
    [Parameter]
    public bool FixedWidth { get; set; }
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
    public string BorderColor { get; set; }

    /// <summary>
    /// Set padding around.
    /// </summary>
    [Parameter]
    public string BorderPadding { get; set; }

    /// <summary>
    /// Set border radius.
    /// </summary>
    [Parameter]
    public string BorderRadius { get; set; }

    /// <summary>
    /// Set border style.
    /// </summary>
    [Parameter]
    public BorderStyle BorderStyle { get; set; }

    /// <summary>
    /// Set border width.
    /// </summary>
    [Parameter]
    public string BorderWidth { get; set; }

    /// <summary>
    /// Padding is used to create space around an element's content, inside of any defined borders.
    /// <para>
    /// top, right, bottom, left
    /// </para>
    /// </summary>
    [Parameter]
    public string Padding { get; set; }

    /// <summary>
    /// Margins are used to create space around elements, outside of any defined borders.
    /// <para>
    /// top, right, bottom, left
    /// </para>
    /// </summary>
    [Parameter]
    public string Margin { get; set; }

    /// <summary>
    /// Inverts the color of an icon to white.
    /// </summary>
    [Parameter]
    public bool Inverse { get; set; }

    /// <summary>
    /// Set color of an inverted icon.
    /// </summary>
    [Parameter]
    public string InverseColor { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> AdditionalAttributes { get; set; }
    #endregion

    /// <summary>
    /// Set to <c>"true"</c> to replace any elements that look like icons with the inline SVG.
    /// Set to <c>"nest"</c> to insert SVGs as child elements.
    /// </summary>
    [Parameter]
    public ProcessingSvgMethod AutoReplaceSvg { get; set; }
    private string _oldAutoReplaceSvg;

    [Inject]
    protected IJSRuntime JSRuntime { get; set; }

    protected override async Task OnParametersSetAsync() {
        await base.OnParametersSetAsync();

        if (AutoReplaceSvg is not ProcessingSvgMethod.NotSet) {
            _oldAutoReplaceSvg = await JSRuntime.SetAutoReplaceSvgAsync(AutoReplaceSvg.GetStringValue()).ConfigureAwait(false);
        }

        AdditionalAttributes ??= new();

        _ = AdditionalAttributes.AddOrUpdateIf("class", BuildClasses()?.Trim(), value => !string.IsNullOrEmpty(Convert.ToString(value)));
        _ = AdditionalAttributes.AddOrUpdateIf("style", BuildStyles()?.Trim(), value => !string.IsNullOrEmpty(Convert.ToString(value)));
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        await base.OnAfterRenderAsync(firstRender);

        if (!string.IsNullOrEmpty(_oldAutoReplaceSvg)) {
            _ = await JSRuntime.SetAutoReplaceSvgAsync(_oldAutoReplaceSvg).ConfigureAwait(false);
            _oldAutoReplaceSvg = null;
        }
    }

    protected string BuildClasses() {
        var classes = new List<string>() {
                            CssClass
                        };

        if (FixedWidth) {
            classes.Add("fa-fw");
        }

        var iconSizeCss = IconSize.GetStringValue();

        if (!string.IsNullOrEmpty(iconSizeCss)) {
            classes.Add(iconSizeCss);
        }

        if (Bordered) {
            classes.Add("fa-border");
        }

        if (Inverse) {
            classes.Add("fa-inverse");
        }

        classes = AddClasses(classes);

        return string.Join(' ', classes.Where(x => !string.IsNullOrEmpty(x))).Trim();
    }
    protected virtual List<string> AddClasses(List<string> classes) => classes;

    protected string BuildStyles() {
        var styles = new Dictionary<string, string>();

        // ===== Check exist Style =====
        if (AdditionalAttributes.TryGetValue("style", out var existStyles)) {
            var existStylesString = Convert.ToString(existStyles);
            foreach (var style in existStylesString.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)) {
                var kvp = style.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (kvp.Length == 2) {
                    styles.Add(kvp[0], kvp[1]);
                }
            }
        }

        styles = styles.AddOrUpdate("--fa-inverse", InverseColor);

        if (Bordered) {
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

    //protected string BuildStyles() {
    //    var styles = new Dictionary<string, string>();

    //    // ===== Check exist Style =====
    //    if (AdditionalAttributes.TryGetValue("style", out var existStyles)) {
    //        var existStylesString = Convert.ToString(existStyles);
    //        foreach (var style in existStylesString.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) {
    //            var kvp = style.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    //            if (kvp.Length == 2) {
    //                _ = styles.Add(kvp[0], kvp[1]);
    //            }
    //        }
    //    }

    //    styles = styles.AddIfNotNull("--fa-inverse", InverseColor);

    //    if (Bordered) {
    //        styles = styles.AddIfNotNull("--fa-border-color", BorderColor)
    //                       .AddIfNotNull("--fa-border-padding", BorderPadding)
    //                       .AddIfNotNull("--fa-border-radius", BorderRadius)
    //                       .AddIf("--fa-border-style", BorderStyle, value => value is not BorderStyle.None)
    //                       .AddIfNotNull("--fa-border-width", BorderWidth);
    //    }

    //    styles = styles.AddIfNotNull("background-color", BackgroundColor)
    //                   .AddIfNotNull("color", Color)
    //                   .AddIfNotNull("padding", Padding)
    //                   .AddIfNotNull("margin", Margin);

    //    styles = AddStyles(styles);

    //    return styles.JoinAsStyles();
    //}

    protected virtual Dictionary<string, string> AddStyles(Dictionary<string, string> styles) => styles;

    protected sealed override void BuildRenderTree(RenderTreeBuilder builder) => BuildRenderTree(builder, GetBaseSequence());
    protected virtual void BuildRenderTree(RenderTreeBuilder builder, int sequence) { }

    protected virtual int BuildElementRenderTree(ref int sequence, RenderTreeBuilder builder) {
        builder.AddMultipleAttributes(sequence++, AdditionalAttributes);

        return sequence;
    }

    //protected override bool ShouldRender() => ForceRebuildRenderTree;
    protected int GetBaseSequence() => ForceRebuildRenderTree ? new Random().Next(0, 5000) : 0;
}
