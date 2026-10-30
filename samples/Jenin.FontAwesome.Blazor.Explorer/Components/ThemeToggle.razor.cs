//
// Copyright (c) Jeninnet.
// Part of the Jeninnet Platform.
// Repository: https://github.com/TarekNajem04/Jenin.FontAwesome.Blazor/
// Licensed under the MIT License.
//
namespace Jenin.FontAwesome.Blazor.Explorer.Components;

/// <summary>
/// The light/dark theme switch. It is presentational: the owning <see cref="Explorer"/> holds the
/// theme state, persists the choice, and stamps it on the explorer root; this control only reflects
/// the current value and reports the user's intent.
/// </summary>
public partial class ThemeToggle {
    /// <summary>
    /// Gets or sets a value indicating whether the dark palette is currently active.
    /// </summary>
    [Parameter]
    public bool IsDark { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the switch is rendered. A host page that owns the
    /// theme itself hides the switch so the user only deals with a single control.
    /// </summary>
    [Parameter]
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Gets or sets the callback invoked when the active theme changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> ThemeChanged { get; set; }

    private Task ToggleAsync() => ThemeChanged.InvokeAsync(!IsDark);
}
