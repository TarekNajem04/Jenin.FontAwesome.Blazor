namespace Jenin.FontAwesome.Blazor.Components;

public class Icon : ElemenWithTransformations {
    /// <summary>
    /// Set icon style.
    /// </summary>
    [Parameter]
    public Typefaces Typeface { get; set; }

    /// <summary>
    /// Set icon style.
    /// </summary>
    [Parameter]
    public IconStyle IconStyle { get; set; }

    [Parameter]
    public string IconName { get; set; }

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
    /// To create animations like beat, fade, beat-fade, flip, and spin to your icons,
    /// so that you can create more visual interest on your site.
    /// </summary>
    [Parameter]
    public Animation Animation { get; set; }

    [Parameter]
    public PullPosition PullPosition { get; set; }

    /// <summary>
    /// Set margin around icon.
    /// </summary>
    [Parameter]
    public string PullMargin { get; set; }

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
    public string MaskIconName { get; set; }

    [Parameter]
    public string MaskId { get; set; }

    /// <summary>
    /// Set duotone primary layer color
    /// </summary>
    [Parameter]
    public string PrimaryColor { get; set; }

    /// <summary>
    /// Set duotone primary layer opacity (between 0 and 1)
    /// </summary>
    [Parameter]
    public float? PrimaryOpacity { get; set; }

    /// <summary>
    /// Set duotone secondary layer color
    /// </summary>
    [Parameter]
    public string SecondaryColor { get; set; }

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

    protected string GetMaskIcon() => !string.IsNullOrEmpty(MaskIconName) ? $"{MaskIconStyle.GetStringValue()} {MaskIconName}" : null;
    protected string GetMaskId() => !string.IsNullOrEmpty(MaskId) ? MaskId : null;
    protected bool IsNested => CombiningRotate is not IconRotate.NotSet;

    protected override void OnInitialized() {
        base.OnInitialized();

        AdditionalAttributes ??= new();

        _ = AdditionalAttributes.UpdateOrRemoveIf("data-fa-mask", GetMaskIcon(), value => string.IsNullOrEmpty(Convert.ToString(value)));
        _ = AdditionalAttributes.UpdateOrRemoveIf("data-fa-mask-id", GetMaskId(), value => string.IsNullOrEmpty(Convert.ToString(value)));
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder, int sequence) {
        /*
            @if(IsNested) {
                <span class=@BuildContainerClasses() style=@BuildContainerStyles()>
                    <i @attributes=AdditionalAttributes
                       @onclick=@OnClickHandlerAsync
                       @ondblclick=@OnDoubleClickHandlerAsync
                       @onmouseover=@OnMouseOverHandlerAsync
                       @onmouseout=@OnMouseOutHandlerAsync>
                    </i>
                </span>
            } else {
                <i @attributes=AdditionalAttributes
                   @onclick=@OnClickHandlerAsync
                   @ondblclick=@OnDoubleClickHandlerAsync
                   @onmouseover=@OnMouseOverHandlerAsync
                   @onmouseout=@OnMouseOutHandlerAsync>
                </i>
            }
        */

        if (IsNested) {
            builder.OpenElement(sequence, "span");
            builder.AddAttribute(sequence++, "class", BuildContainerClasses());
            builder.AddAttribute(sequence++, "style", BuildContainerStyles());
            sequence++;
        }

        builder.OpenElement(sequence, "i");
        _ = BuildElementRenderTree(ref sequence, builder);

        builder.CloseElement();

        // close span tag
        if (IsNested) {
            builder.CloseElement();
        }
    }

    protected override List<string> AddClasses(List<string> classes) {
        classes = base.AddClasses(classes);
        classes.AddRange
                (
                    new[]
                    {
                        Typeface.GetStringValue(),
                        IconStyle.GetStringValue(),
                        IconName,
                        Rotate.GetStringValue(),
                        Animation?.BuildClasses(),
                        IsNested ? null : BuildContainerClasses(),
                        StackSize.GetStringValue(),
                        SwapOpacity ? "fa-swap-opacity" : null,
                    }
                );

        return classes;
    }

    protected override Dictionary<string, string> AddStyles(Dictionary<string, string> styles) {
        styles = base.AddStyles(styles);

        if (Rotate is IconRotate.RotateBy && RotateAngle.HasValue) {
            _ = styles.AddOrUpdate("--fa-rotate-angle", $"{RotateAngle.Value.ToString("F2", CultureInfo.InvariantCulture)}deg");
        }

        _ = Animation?.AddStyles(styles);

        if (!IsNested) {
            styles = BuildContainerStyles(styles);
        }

        return styles.AddOrUpdateIfNotNull("--fa-stack-z-index", StackZIndex)
                     .AddOrUpdateIfNotNull("--fa-primary-color", PrimaryColor)
                     .AddOrUpdateIfNotNull("--fa-primary-opacity;", PrimaryOpacity = PrimaryOpacity.HasValue ? MathF.Min(1, Math.Max(0, PrimaryOpacity.Value)) : PrimaryOpacity)
                     .AddOrUpdateIfNotNull("--fa-secondary-color", SecondaryColor)
                     .AddOrUpdateIfNotNull("--fa-secondary-opacity", SecondaryOpacity = SecondaryOpacity.HasValue ? MathF.Min(1, Math.Max(0, SecondaryOpacity.Value)) : SecondaryOpacity);
    }

    protected virtual string BuildContainerClasses()
        => string.Join
                  (
                    ' ',
                    CombiningRotate.GetStringValue(),
                    PullPosition is not PullPosition.None ? PullPosition.GetStringValue() : null
                  )
                 .Trim();

    protected virtual string BuildContainerStyles() => BuildContainerStyles(new Dictionary<string, string>()).JoinAsStyles();

    protected virtual Dictionary<string, string> BuildContainerStyles(Dictionary<string, string> styles) {
        if (IsNested) {
            _ = styles.AddOrUpdateIfNotNull("display", "inline-block");
        }

        if (CombiningRotate is IconRotate.RotateBy && RotateAngle.HasValue) {
            _ = styles.AddOrUpdate("--fa-rotate-angle", $"{RotateAngle.Value.ToString("F2", CultureInfo.InvariantCulture)}deg");
        }

        if (PullPosition is not PullPosition.None && !string.IsNullOrEmpty(PullMargin)) {
            _ = styles.AddOrUpdate("--fa-pull-margin", PullMargin);
        }

        return styles;
    }
}
