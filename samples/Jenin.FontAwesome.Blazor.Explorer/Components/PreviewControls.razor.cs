//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//

namespace Jenin.FontAwesome.Blazor.Explorer.Components;

/// <summary>
/// The live preview controls. Each input mutates the shared <see cref="ExplorerPreviewState"/> and
/// then raises <see cref="OnChanged"/> so the owning preview panel re-renders the icon.
/// </summary>
/// <remarks>
/// Numeric inputs are bound explicitly rather than through <c>@bind</c>: Blazor's implicit binding
/// parses and formats with the current culture, so on a server whose culture uses a different decimal
/// separator a range value such as <c>1.1</c> can be read as <c>11</c>. Every value here is parsed and
/// formatted with <see cref="CultureInfo.InvariantCulture"/> so the control and the icon agree on the
/// number regardless of the host culture.
/// </remarks>
public partial class PreviewControls {
    /// <summary>
    /// Gets or sets the preview state mutated by the controls.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public ExplorerPreviewState State { get; set; } = default!;

    /// <summary>
    /// Gets or sets the callback raised after any control value changes.
    /// </summary>
    [Parameter]
    public EventCallback OnChanged { get; set; }

    private Task NotifyAsync() => OnChanged.InvokeAsync();

    private Task OnToggleColorAsync(ChangeEventArgs args) {
        var enabled = args.Value is true;
        State.UseColor = enabled;

        // Returning to defaults when the colour is switched off keeps the picker and the icon in
        // agreement: the icon drops the inline colour and the swatch shows the default again.
        if(!enabled) {
            State.Color = ExplorerPreviewState.DEFAULT_COLOR;
        }

        return NotifyAsync();
    }

    private Task OnColorInput(ChangeEventArgs args) {
        if(args.Value is string value && !string.IsNullOrWhiteSpace(value)) {
            State.Color = value;
        }

        return NotifyAsync();
    }

    private Task OnToggleBackgroundAsync(ChangeEventArgs args) {
        var enabled = args.Value is true;
        State.UseBackgroundColor = enabled;

        if(!enabled) {
            State.BackgroundColor = ExplorerPreviewState.DEFAULT_BACKGROUND_COLOR;
        }

        return NotifyAsync();
    }

    private Task OnBackgroundInput(ChangeEventArgs args) {
        if(args.Value is string value && !string.IsNullOrWhiteSpace(value)) {
            State.BackgroundColor = value;
        }

        return NotifyAsync();
    }

    /// <summary>
    /// Formats a numeric value for a range or number input using the invariant culture so the browser
    /// receives a decimal point it can parse with the input's own <c>step</c>.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <returns>The invariant string form.</returns>
    private static string FormatNumber(float value) => value.ToString("0.####", CultureInfo.InvariantCulture);

    /// <summary>
    /// Parses a numeric input value with the invariant culture and applies it. Non-numeric input is
    /// ignored so a partially typed value never zeroes the state.
    /// </summary>
    /// <param name="args">The change event.</param>
    /// <param name="apply">The setter that receives the parsed value.</param>
    /// <returns>A task that completes when the owning panel has re-rendered.</returns>
    private Task OnNumberInputAsync(ChangeEventArgs args, Action<float> apply) {
        if(args.Value is string value
            && float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed)) {
            apply(parsed);
        }

        return NotifyAsync();
    }

    private Task OnAnimationChangedAsync(ChangeEventArgs args) {
        if(args.Value is string value && Enum.TryParse<PreviewAnimation>(value, out var animation)) {
            State.Animation = animation;
            State.ResetAnimationOptions();
        }

        return NotifyAsync();
    }

    private Task OnToggleDurationAsync(ChangeEventArgs args) {
        State.UseAnimationDuration = args.Value is true;

        if(!State.UseAnimationDuration) {
            State.AnimationDuration = ExplorerPreviewState.DEFAULT_ANIMATION_DURATION;
        }

        return NotifyAsync();
    }

    private Task OnToggleIterationsAsync(ChangeEventArgs args) {
        State.UseAnimationIterationCount = args.Value is true;

        if(!State.UseAnimationIterationCount) {
            State.AnimationIterationCount = ExplorerPreviewState.DEFAULT_ANIMATION_ITERATION_COUNT;
        }

        return NotifyAsync();
    }

    private Task OnResetAsync() {
        State.Reset();
        return NotifyAsync();
    }
}
