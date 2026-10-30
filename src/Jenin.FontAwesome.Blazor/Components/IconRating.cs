//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// A keyboard-operable star rating input with a fractional readonly display. In interactive mode
/// every option is a native radio, so arrow keys, Home/End, focus movement, and checked semantics
/// work without JavaScript; readonly values render as a clipped overlay that supports fractions.
/// </summary>
/// <remarks>
/// <para>
/// Interactive input snaps to whole stars. Fractional <see cref="Value"/> values display floored
/// (for example 2.5 lights two stars); use readonly mode (<see cref="Interactive"/> set to
/// <see langword="false"/>) to present fractional values.
/// </para>
/// </remarks>
public class IconRating : Element {
    /// <summary>
    /// The well-known <c>aria-label</c> attribute name.
    /// </summary>
    private const string AriaLabelAttribute = "aria-label";

    /// <summary>
    /// The well-known <c>style</c> attribute name.
    /// </summary>
    private const string StyleAttribute = "style";

    private string? _groupName;
    private int? _focusedOption;

    /// <summary>
    /// The current rating. Interactive input snaps to whole numbers in the inclusive range
    /// 0 to <see cref="Max"/>; 0 means no selection.
    /// </summary>
    [Parameter]
    public float Value { get; set; }

    /// <summary>
    /// Occurs when <see cref="Value"/> changes through user interaction. Supports two-way binding.
    /// </summary>
    [Parameter]
    public EventCallback<float> ValueChanged { get; set; }

    /// <summary>
    /// An expression identifying the bound value. Supplied automatically when <c>@bind-Value</c>
    /// is used; not consumed directly.
    /// </summary>
    [Parameter]
    public Expression<Func<float>>? ValueExpression { get; set; }

    /// <summary>
    /// The number of rating options.
    /// </summary>
    [Parameter]
    public int Max { get; set; } = 5;

    /// <summary>
    /// Gets or sets a value indicating whether the rating accepts input. When
    /// <see langword="false"/> the value renders as a fractional overlay and ignores input.
    /// </summary>
    [Parameter]
    public bool Interactive { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the button is disabled. Applies to interactive mode.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a clear control is offered when a value is selected.
    /// </summary>
    [Parameter]
    public bool AllowDeselect { get; set; } = true;

    /// <summary>
    /// The glyph used for every option.
    /// </summary>
    [Parameter]
    public string? IconName { get; set; } = "fa-star";

    /// <summary>
    /// The icon style used for unselected options.
    /// </summary>
    [Parameter]
    public IconStyle EmptyIconStyle { get; set; } = IconStyle.Regular;

    /// <summary>
    /// The icon style used for selected options.
    /// </summary>
    [Parameter]
    public IconStyle FullIconStyle { get; set; } = IconStyle.Solid;

    /// <summary>
    /// The color applied to selected options.
    /// </summary>
    [Parameter]
    public string? FullColor { get; set; } = "#f59e0b";

    /// <summary>
    /// Accessible name for the rating group. Falls back to <see cref="ElementBase.Title"/>.
    /// </summary>
    [Parameter]
    public string? RatingLabel { get; set; }

    /// <summary>
    /// Accessible name for the rating's clear control.
    /// </summary>
    [Parameter]
    public string? ClearLabel { get; set; } = "Clear rating";

    private int OptionCount => Math.Max(1, Max);

    private float Clamped => MathF.Min(OptionCount, MathF.Max(0, Value));

    private bool IsSelected(int option) => Clamped >= 1 && (int)MathF.Round(Clamped) == option;

    private bool IsFull(int option) => Clamped >= option;

    private bool IsTabbable(int option) => IsSelected(option) || (Clamped < 1 && option == 1);

    /// <inheritdoc />
    protected override void OnInitialized() =>
        _groupName = $"jenin-rating-{Guid.NewGuid():N}";

    private protected override void BuildAccessibilityAttributes() {
        AdditionalAttributes ??= [];

        if(Interactive) {
            _ = AdditionalAttributes.AddOrUpdate("role", "radiogroup");
        } else {
            _ = AdditionalAttributes.AddOrUpdate("role", "img");
            _ = AdditionalAttributes.AddOrUpdate(AriaLabelAttribute, $"{FormatValue()} out of {OptionCount}");
        }

        var name = RatingLabel ?? Title;

        if(Interactive && !string.IsNullOrWhiteSpace(name)) {
            _ = AdditionalAttributes.AddOrUpdate(AriaLabelAttribute, name.Trim());
        }

        if(!string.IsNullOrWhiteSpace(Title)) {
            _ = AdditionalAttributes.AddOrUpdate("title", Title.Trim());
        }
    }

    private static string FormatValue(float value, int max) {
        var clamped = MathF.Min(max, MathF.Max(0, value));

        return clamped.ToString("0.##", CultureInfo.InvariantCulture);
    }

    private string FormatValue() => FormatValue(Clamped, OptionCount);

    private protected override Dictionary<string, string> AddStyles(Dictionary<string, string> styles) {
        styles = base.AddStyles(styles);

        styles["position"] = "relative";
        styles["display"] = "inline-flex";
        styles["align-items"] = "center";

        return styles;
    }

    private async Task OnOptionChangedAsync(ChangeEventArgs args) {
        ArgumentNullException.ThrowIfNull(args);

        if(!Interactive || Disabled) {
            return;
        }

        if(int.TryParse(Convert.ToString(args.Value, CultureInfo.InvariantCulture), out var option)) {
            await SetValueAsync(Math.Clamp(option, 1, OptionCount));
        }
    }

    private async Task ClearAsync() {
        if(!Interactive || Disabled) {
            return;
        }

        await SetValueAsync(0);
    }

    private async Task SetValueAsync(float value) {
        if(Math.Abs(Value - value) < float.Epsilon) {
            return;
        }

        Value = value;

        await ValueChanged.InvokeAsync(value);
    }

    /// <summary>
    /// Builds the component's render tree: native radios with custom glyph labels in interactive
    /// mode, or a clipped fractional overlay in readonly mode.
    /// </summary>
    /// <param name="builder">The render tree builder.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);

        builder.OpenElement(0, "span");
        AddAdditionalAttributesToRenderTree(builder, sequence: 1);
        AddEventsToRenderTree(builder, sequence: 2);

        if(Interactive) {
            BuildInteractive(builder);
        } else {
            BuildReadonly(builder);
        }

        builder.CloseElement();
    }

    private void BuildInteractive(RenderTreeBuilder builder) {
        for(var option = 1; option <= OptionCount; option++) {
            var current = option;
            var selected = IsSelected(current);

            builder.OpenElement(10 + (current * 10), "label");
            builder.AddAttribute(11 + (current * 10), StyleAttribute, FocusStyle(current) + "cursor:pointer;display:inline-flex;");
            builder.AddAttribute(12 + (current * 10), "title", $"{current} of {OptionCount}");

            builder.OpenElement(13 + (current * 10), "input");
            builder.AddAttribute(14 + (current * 10), "type", "radio");
            builder.AddAttribute(15 + (current * 10), "name", _groupName);
            builder.AddAttribute(16 + (current * 10), "value", current);
            builder.AddAttribute(17 + (current * 10), "checked", selected);
            builder.AddAttribute(18 + (current * 10), "disabled", Disabled);
            builder.AddAttribute(19 + (current * 10), AriaLabelAttribute, $"{current} of {OptionCount}");
            builder.AddAttribute(20 + (current * 10), StyleAttribute, "position:absolute;width:1px;height:1px;margin:-1px;padding:0;overflow:hidden;clip:rect(0 0 0 0);white-space:nowrap;border:0;");
            builder.AddAttribute(21 + (current * 10), "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, OnOptionChangedAsync));
            builder.AddAttribute(22 + (current * 10), "onfocus", EventCallback.Factory.Create<FocusEventArgs>(this, () => _focusedOption = current));
            builder.AddAttribute(23 + (current * 10), "onblur", EventCallback.Factory.Create<FocusEventArgs>(this, () => _focusedOption = null));
            builder.AddAttribute(24 + (current * 10), "tabindex", IsTabbable(current) ? "0" : "-1");
            builder.CloseElement();

            builder.OpenComponent<Icon>(24 + (current * 10));
            builder.AddAttribute(25 + (current * 10), nameof(Icon.IconName), IconName);
            builder.AddAttribute(26 + (current * 10), nameof(Icon.IconStyle), IsFull(current) ? FullIconStyle : EmptyIconStyle);

            if(IsFull(current) && !string.IsNullOrEmpty(FullColor)) {
                builder.AddAttribute(27 + (current * 10), nameof(Icon.Color), FullColor);
            }

            builder.AddAttribute(28 + (current * 10), nameof(Icon.Decorative), true);
            builder.CloseComponent();
            builder.CloseElement();
        }

        if(AllowDeselect && Clamped >= 1 && !Disabled) {
            builder.OpenElement(1000, "button");
            builder.AddAttribute(1001, "type", "button");
            builder.AddAttribute(1002, AriaLabelAttribute, ClearLabel ?? "Clear rating");
            builder.AddAttribute(1003, "title", ClearLabel ?? "Clear rating");
            builder.AddAttribute(1004, StyleAttribute, "background:none;border:0;padding:0 0.25em;cursor:pointer;color:inherit;font-size:0.8em;");
            builder.AddAttribute(1005, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, ClearAsync));
            builder.AddContent(1006, "×");
            builder.CloseElement();
        }
    }

    private string FocusStyle(int option) =>
        _focusedOption == option ? "outline:2px solid currentColor;outline-offset:2px;border-radius:0.25em;" : string.Empty;

    private void BuildReadonly(RenderTreeBuilder builder) {
        var percent = (Clamped / OptionCount * 100).ToString("F2", CultureInfo.InvariantCulture);

        builder.OpenElement(2000, "span");
        builder.AddAttribute(2001, "aria-hidden", "true");
        builder.AddAttribute(2002, StyleAttribute, "display:inline-flex;white-space:nowrap;");

        for(var option = 1; option <= OptionCount; option++) {
            builder.OpenComponent<Icon>(2003 + option);
            builder.AddAttribute(2004 + option, nameof(Icon.IconName), IconName);
            builder.AddAttribute(2005 + option, nameof(Icon.IconStyle), EmptyIconStyle);
            builder.AddAttribute(2006 + option, nameof(Icon.Decorative), true);
            builder.CloseComponent();
        }

        builder.CloseElement();

        builder.OpenElement(2100, "span");
        builder.AddAttribute(2101, "aria-hidden", "true");
        builder.AddAttribute(2102, StyleAttribute, $"position:absolute;inset-block-start:0;inset-inline-start:0;width:{percent}%;overflow:hidden;white-space:nowrap;color:{Color ?? "inherit"};");

        for(var option = 1; option <= OptionCount; option++) {
            builder.OpenComponent<Icon>(2103 + option);
            builder.AddAttribute(2104 + option, nameof(Icon.IconName), IconName);
            builder.AddAttribute(2105 + option, nameof(Icon.IconStyle), FullIconStyle);
            builder.AddAttribute(2106 + option, nameof(Icon.Decorative), true);
            builder.CloseComponent();
        }

        builder.CloseElement();
    }
}
