//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Components;

/// <summary>
/// A button that toggles between two glyph states (for example bell on/off or heart outline/filled),
/// exposing the standard <c>aria-pressed</c> contract and two-way <see cref="Pressed"/> binding.
/// </summary>
/// <remarks>
/// <para>
/// Clicking the button flips <see cref="Pressed"/> before invoking <see cref="Element.OnClick"/>,
/// so a single handler observes the new state. Pressed-state parameters (<c>Pressed*</c>) fall back
/// to their base counterparts when left unset; only differing values need to be supplied.
/// </para>
/// <para>
/// The default <see cref="PressedAnimation"/> plays the press feedback exactly once; set it to
/// <see langword="null"/> for an instant swap.
/// </para>
/// </remarks>
public class IconToggleButton : IconButton {
    /// <summary>
    /// Gets or sets a value indicating whether the toggle is in its pressed state.
    /// </summary>
    [Parameter]
    public bool Pressed { get; set; }

    /// <summary>
    /// Occurs when <see cref="Pressed"/> changes through user interaction.
    /// </summary>
    [Parameter]
    public EventCallback<bool> PressedChanged { get; set; }

    /// <summary>
    /// The glyph rendered while <see cref="Pressed"/> is set. Defaults to <see cref="IconButton.IconName"/>.
    /// </summary>
    [Parameter]
    public string? PressedIconName { get; set; }

    /// <summary>
    /// The icon style used while <see cref="Pressed"/> is set. Defaults to the base style.
    /// </summary>
    [Parameter]
    public IconStyle? PressedIconStyle { get; set; }

    /// <summary>
    /// The animation played on the glyph while <see cref="Pressed"/> is set. Defaults to a single
    /// beat; set to <see langword="null"/> for an instant swap without animation.
    /// </summary>
    [Parameter]
    public Animation? PressedAnimation { get; set; } = Animate.Beat().SetIterationCount(1);

    /// <summary>
    /// The glyph color applied while <see cref="Pressed"/> is set. Defaults to inheriting the
    /// button color.
    /// </summary>
    [Parameter]
    public string? PressedColor { get; set; }

    /// <summary>
    /// Visible text rendered while <see cref="Pressed"/> is set. Defaults to <see cref="IconButton.Label"/>.
    /// </summary>
    [Parameter]
    public string? PressedLabel { get; set; }

    /// <summary>
    /// Tooltip rendered while <see cref="Pressed"/> is set. Defaults to <see cref="ElementBase.Title"/>.
    /// </summary>
    [Parameter]
    public string? PressedTitle { get; set; }

    /// <summary>
    /// Accessible name used while <see cref="Pressed"/> is set. Defaults to
    /// <see cref="PressedLabel"/>, then <see cref="PressedTitle"/>.
    /// </summary>
    [Parameter]
    public string? PressedAriaLabel { get; set; }

    private protected override string? CurrentLabel => Pressed ? PressedLabel ?? Label : Label;

    private protected override string? CurrentIconName {
        get {
            if(Busy) {
                return BusyIconName;
            }

            return Pressed ? PressedIconName ?? IconName : IconName;
        }
    }

    private protected override IconStyle ForwardIconStyle {
        get {
            if(Busy) {
                return BusyIconStyle;
            }

            return Pressed ? PressedIconStyle ?? base.ForwardIconStyle : base.ForwardIconStyle;
        }
    }

    private protected override Animation? CurrentAnimation {
        get {
            if(Busy) {
                return BusyAnimation;
            }

            return Pressed ? PressedAnimation ?? Animation : Animation;
        }
    }

    private protected override string? CurrentTitle => Pressed ? PressedTitle ?? Title : Title;

    private protected override string? CurrentAriaLabel {
        get {
            if(CurrentLabel is not null) {
                return null;
            }

            return Pressed ? PressedAriaLabel ?? PressedTitle ?? base.CurrentAriaLabel
                           : base.CurrentAriaLabel;
        }
    }

    private protected override string? CurrentIconColor => Pressed ? PressedColor : null;

    /// <summary>
    /// Applies parameter changes before rendering: maintains the <c>aria-pressed</c> attribute
    /// contract for the persistent additional-attributes dictionary.
    /// </summary>
    protected override void OnParametersSet() {
        base.OnParametersSet();

        AdditionalAttributes ??= [];

        _ = AdditionalAttributes.AddOrUpdate("aria-pressed", Pressed ? "true" : "false");
    }

    private protected override void AddEventsToRenderTree(RenderTreeBuilder builder, int sequence) {
        ArgumentNullException.ThrowIfNull(builder);

        builder.AddAttribute(sequence, "onclick", EventCallback.Factory.Create<MouseEventArgs>(this, TogglePressedAsync));

        if(OnDoubleClick.HasDelegate) {
            builder.AddAttribute(sequence + 1, "ondblclick", EventCallback.Factory.Create<MouseEventArgs>(this, OnDoubleClickHandlerAsync));
        }

        if(OnMouseOver.HasDelegate) {
            builder.AddAttribute(sequence + 2, "onmouseover", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseOverHandlerAsync));
        }

        if(OnMouseOut.HasDelegate) {
            builder.AddAttribute(sequence + 3, "onmouseout", EventCallback.Factory.Create<MouseEventArgs>(this, OnMouseOutHandlerAsync));
        }
    }

    private async Task TogglePressedAsync(MouseEventArgs eventArgs) {
        if(Disabled || Busy) {
            return;
        }

        Pressed = !Pressed;

        await PressedChanged.InvokeAsync(Pressed);
        await OnClickHandlerAsync(eventArgs);
    }
}
